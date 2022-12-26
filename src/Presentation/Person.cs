using System;
using System.Linq;
using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;

[SceneReference("Person.tscn")]
public partial class Person
{
    [Export]
    public float MoveSpeed { get; set; } = 128;

    [Export]
    public float MaxThristLevel { get; set; } = 100;

    [Export]
    public float ThristSpeed { get; set; } = 3f;

    [Export]
    public float ThristThreshold { get; set; } = 50;

    private float CurrentThristLevel { get; set; } = 100;

    private ActionExecutor actions = new ActionExecutor();

    public Random r = new Random();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        FindNewTarget();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.CurrentThristLevel -= this.ThristSpeed * delta;
        if (this.CurrentThristLevel < this.ThristThreshold)
        {
            var water = this.GetTree().GetNodesInGroup(Groups.WaterToDrink).Cast<Node2D>().First();
            actions.CancelActions();
            actions.AddAction(new MoveUnitAction(this, water.Position, this.MoveSpeed));
            actions.AddAction(new SleepUnitAction((float)r.NextDouble() * 2));
            actions.AddAction(new CallbackUnitAction(FindNewTarget));

            if (this.Position == water.Position)
            {
                var well = (Well)water;
                var toDrink = Math.Min(MaxThristLevel - CurrentThristLevel, well.CurrentAmount);
                well.CurrentAmount -= toDrink;
                this.CurrentThristLevel += toDrink;
            }
        }

        this.label.Text = this.CurrentThristLevel.ToString("#");

        if (this.CurrentThristLevel <= 0)
        {
            this.QueueFree();
        }

        actions.Process(delta);
    }

    private void FindNewTarget()
    {
        var targetPosition = this.map.MapToGlobal(new Vector2(r.Next(50), r.Next(50)));
        actions.AddAction(new MoveUnitAction(this, targetPosition, this.MoveSpeed));
        actions.AddAction(new SleepUnitAction((float)r.NextDouble() * 2));
        actions.AddAction(new CallbackUnitAction(FindNewTarget));
    }
}
