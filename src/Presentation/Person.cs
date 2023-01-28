using System;
using Godot;
using GodotAnalysers;
using BrainAI.AI.UtilityAI;
using LocomotorECS;

[SceneReference("Person.tscn")]
public partial class Person
{
    public readonly Entity e = BuildEntity();

    public Random r = new Random();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.label.Text = this.e.GetComponent<PrintComponent>().Text + "\n" +
           "Thristing: " + this.e.GetComponent<DrinkThristingComponent>().CurrentThristing.ToString("#") + "\n" +
           "Fatigue: " + this.e.GetComponent<FatigueComponent>().CurrentFatigue.ToString("#") + "\n";
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        e.GetOrCreateComponent<Node2DComponent>().Node = this;
        this.GetParent<Map>().el.Add(e);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        this.GetParent<Map>().el.Remove(e);
    }

    public static Entity BuildEntity()
    {
        var entity = new Entity();
        entity.GetOrCreateComponent<PersonComponent>();
        entity.GetOrCreateComponent<PositionComponent>();
        entity.GetOrCreateComponent<MovingComponent>();
        entity.GetOrCreateComponent<DrinkThristingComponent>();
        entity.GetOrCreateComponent<PrintComponent>();
        entity.GetOrCreateComponent<DyingComponent>();
        entity.GetOrCreateComponent<BuilderComponent>();
        entity.GetOrCreateComponent<PersonDecisionWalkComponent>();
        entity.GetOrCreateComponent<FatigueComponent>().MaxFatigue = 100;
        entity.GetOrCreateComponent<FatigueComponent>().FatigueThreshold = 80;
        entity.GetOrCreateComponent<FatigueComponent>().FatigueSpeed = 1f;
        entity.GetOrCreateComponent<FatigueComponent>().DefaultRest = 5f;
        return entity;
    }
}
