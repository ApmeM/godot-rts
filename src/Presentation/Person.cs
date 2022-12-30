using System;
using System.Linq;
using BrainAI.Pathfinding;
using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;
using BrainAI.AI.UtilityAI;

[SceneReference("Person.tscn")]
public partial class Person
{

    public class Context
    {
        public Node2D Node;

        public float delta;

        public ActionExecutor Actions = new ActionExecutor();

        public float ThristThreshold { get; set; } = 50;

        public float CurrentThristLevel { get; set; } = 100;

        public float MaxThristLevel { get; set; } = 100;

        public bool GoingToDrink { get; set; } = false;
    }

    [Export]
    public float MoveSpeed { get; set; } = 128;

    [Export]
    public float ThristSpeed { get; set; } = 3f;

    public Random r = new Random();

    private Context context;

    private UtilityAI<Context> ai;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.context = new Context
        {
            Node = this
        };

        var reasoner = new HighestScoreReasoner<Context>();
        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.MaxThristLevel - a.CurrentThristLevel),
                new ActionAppraisal<Context>(a => a.GoingToDrink ? 0 : 1)
                ),
                new PrintAction<Context>("Looking for water"),
                new ActionAction<Context>(a =>
                {
                    a.GoingToDrink = true;
                    var well = a.Node.GetTree().GetNodesInGroup(Groups.WaterToDrink).Cast<Well>().First();
                    var map = a.Node.GetParent<Map>();
                    var currentMap = map.GlobalToMap(a.Node.Position);
                    var onMap = map.GlobalToMap(well.Position);
                    var result = AStarPathfinder.Search(map.graph, currentMap, onMap);
                    a.Actions.CancelActions();
                    foreach (var cell in result)
                    {
                        var targetPosition = this.map.MapToGlobal(cell);
                        a.Actions.AddAction(new MoveUnitAction(this, targetPosition, this.MoveSpeed));
                    }
                    a.Actions.AddAction(new DrinkUnitAction(context, well));
                }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.GoingToDrink ? 1 : 0),
                new FixedAppraisal<Context>(100)
                ),
                new PrintAction<Context>("Going to drink"),
                new ActionAction<Context>(a =>
                {
                    a.Actions.Process(a.delta);
                    if (a.Actions.IsEmpty())
                    {
                        a.GoingToDrink = false;
                    }
                }));


        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.CurrentThristLevel <= 0 ? 1 : 0),
                new FixedAppraisal<Context>(1000)
                ),
                new PrintAction<Context>("Dying"),
                new ActionAction<Context>(a =>
                {
                    a.Node.QueueFree();
                }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new FixedAppraisal<Context>(50),
                new ActionAppraisal<Context>(a => a.Actions.IsEmpty() ? 1 : 0)
                ),
                new PrintAction<Context>("Thinking"),
                new ActionAction<Context>(a =>
                {
                    var onMap = new Vector2(r.Next(25), r.Next(25));
                    var map = a.Node.GetParent<Map>();
                    var currentMap = map.GlobalToMap(this.Position);
                    var result = AStarPathfinder.Search(map.graph, currentMap, onMap);
                    if (result != null)
                    {
                        foreach (var cell in result)
                        {
                            var targetPosition = this.map.MapToGlobal(cell);
                            a.Actions.AddAction(new MoveUnitAction(this, targetPosition, this.MoveSpeed));
                        }
                        a.Actions.AddAction(new SleepUnitAction((float)r.NextDouble() * 2));
                    }
                }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new FixedAppraisal<Context>(50),
                new ActionAppraisal<Context>(a => a.Actions.IsEmpty() ? 0 : 1)
                ),
                new PrintAction<Context>("Walking"),
                new ActionAction<Context>(a =>
                {
                    a.Actions.Process(a.delta);
                }));


        this.ai = new UtilityAI<Context>(this.context, reasoner);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.context.CurrentThristLevel -= this.ThristSpeed * delta;
        this.context.delta = delta;

        ai.Tick();

        this.label.Text = this.context.CurrentThristLevel.ToString("#");
    }
}

public class PrintAction<T> : IAction<T>
{
    private string text;

    public PrintAction(string text)
    {
        this.text = text;
    }

    public void Enter(T context)
    {
    }

    public void Execute(T context)
    {
        GD.Print(text);
    }

    public void Exit(T context)
    {
    }
}

public class DrinkUnitAction : IUnitAction
{
    private readonly Person.Context context;
    private readonly Well well;

    public DrinkUnitAction(Person.Context context, Well well)
    {
        this.context = context;
        this.well = well;
    }

    public bool Process(float delta)
    {
        var toDrink = Math.Min(context.MaxThristLevel - context.CurrentThristLevel, well.CurrentAmount);
        well.CurrentAmount -= toDrink;
        context.CurrentThristLevel += toDrink;
        return true;
    }
}

