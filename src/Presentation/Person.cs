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

    public Random r = new Random();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.targetPosition = this.Position;
    }

    private Vector2 targetPosition;

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.CurrentThristLevel -= this.ThristSpeed * delta;
        if (this.CurrentThristLevel < this.ThristThreshold)
        {
            var water = this.GetTree().GetNodesInGroup(Groups.WaterToDrink).Cast<Node2D>().First();
            this.targetPosition = water.Position;

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

        if (this.targetPosition != this.Position)
        {
            if ((this.targetPosition - this.Position).LengthSquared() < MoveSpeed * MoveSpeed * delta * delta)
            {
                this.Position = this.targetPosition;
            }
            else
            {
                this.Position = this.Position + delta * MoveSpeed * ((this.targetPosition - this.Position).Normalized());
            }
        }
        else
        {
            if (r.NextDouble() < 0.1)
            {
                this.targetPosition = this.tileMap.MapToGlobal(new Vector2(r.Next(50), r.Next(50)));
            }
        }
    }
}
