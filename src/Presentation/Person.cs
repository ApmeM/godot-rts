using System;
using System.Linq;
using BrainAI.Pathfinding;
using Godot;
using GodotAnalysers;
using BrainAI.AI.UtilityAI;

[SceneReference("Person.tscn")]
public partial class Person
{
    public class Context : TileMapObject.Context, IPrintActionContext
    {
        public string CurrentActionName { get; set; }

        public float Delta;

        public ActionExecutor Actions = new ActionExecutor();

        public bool IsDead;

        public float ThinkTimeout;

        public float ThristThreshold { get; set; } = 50;

        public float CurrentThristLevel { get; set; } = 100;

        public float MaxThristLevel { get; set; } = 100;

        public bool GoingToDrink { get; set; } = false;

        public float MoveSpeed { get; set; } = 128;

        public float ThristSpeed { get; set; } = 3f;

        public void Tick(float delta)
        {
            this.CurrentThristLevel -= this.ThristSpeed * delta;
            this.Delta = delta;

        }
    }

    public Random r = new Random();

    private Context myContext => (Context)this.context;

    private UtilityAI<Context> ai;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.myContext.Tick(delta);
        this.ai.Tick();

        if (this.myContext.IsDead)
        {
            this.QueueFree();
        }

        this.label.Text = this.myContext.CurrentActionName + " " + this.myContext.CurrentThristLevel.ToString("#");
    }

    public override void InitContext(Map.Context mapContext)
    {
        this.context = this.context ?? new Context();
        base.InitContext(mapContext);
        this.ai = new UtilityAI<Context>(this.myContext, InitAI());
    }

    public Reasoner<Context> InitAI()
    {
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
                    var well = a.MapContext.KnownPositions.Keys.OfType<Well.Context>().First();
                    var personMap = a.MapContext.WorldToMap(this.Position);
                    var wellMap = a.MapContext.WorldToMap(well.Position);
                    var result = AStarPathfinder.Search(a.MapContext, personMap, wellMap);
                    a.Actions.CancelActions();
                    foreach (var cell in result)
                    {
                        var targetPosition = a.MapContext.MapToWorld(cell);
                        a.Actions.AddAction(new MoveUnitAction(a, targetPosition, a.MoveSpeed));
                    }
                    a.Actions.AddAction(new DrinkUnitAction(a, well));
                }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.GoingToDrink ? 1 : 0),
                new FixedAppraisal<Context>(100)
                ),
                new PrintAction<Context>("Going to drink"),
                new ActionAction<Context>(a =>
                {
                    a.Actions.Process(a.Delta);
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
                    a.IsDead = true;
                }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new FixedAppraisal<Context>(50),
                new ActionAppraisal<Context>(a => a.Actions.IsEmpty() ? 1 : 0)
                ),
                new PrintAction<Context>("Thinking"),
                new ActionAction<Context>(
                    a => { a.ThinkTimeout = this.r.Next(3); },
                    a =>
                    {
                        a.ThinkTimeout -= a.Delta;
                        if (a.ThinkTimeout > 0)
                        {
                            return;
                        }

                        var targetMap = new Vector2(r.Next(25), r.Next(25));
                        var personMap = a.MapContext.WorldToMap(this.Position);
                        var result = AStarPathfinder.Search(a.MapContext, personMap, targetMap);
                        if (result != null)
                        {
                            foreach (var cell in result)
                            {
                                var targetPosition = a.MapContext.MapToWorld(cell);
                                a.Actions.AddAction(new MoveUnitAction(a, targetPosition, a.MoveSpeed));
                            }
                        }
                    },
                    a => { }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new FixedAppraisal<Context>(50),
                new ActionAppraisal<Context>(a => a.Actions.IsEmpty() ? 0 : 1)
                ),
                new PrintAction<Context>("Walking"),
                new ActionAction<Context>(a =>
                {
                    a.Actions.Process(a.Delta);
                }));

        return reasoner;
    }
}

public class DrinkUnitAction : IUnitAction
{
    private readonly Person.Context context;
    private readonly Well.Context well;

    public DrinkUnitAction(Person.Context context, Well.Context well)
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

