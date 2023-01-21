using System;
using Godot;
using GodotAnalysers;
using BrainAI.AI.UtilityAI;
using LocomotorECS;

[SceneReference("Person.tscn")]
public partial class Person
{
    public readonly Entity e = new Entity();

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

        e.GetOrCreateComponent<PersonComponent>();
        e.GetOrCreateComponent<Node2DComponent>().Node = this;
        e.GetOrCreateComponent<PositionComponent>().Position = this.Position;
        e.GetOrCreateComponent<MovingComponent>();
        e.GetOrCreateComponent<DrinkThristingComponent>();
        e.GetOrCreateComponent<PrintComponent>();
        e.GetOrCreateComponent<DyingComponent>();
        e.GetOrCreateComponent<BuilderComponent>();
        e.GetOrCreateComponent<PersonDecisionWalkComponent>();
        e.GetOrCreateComponent<FatigueComponent>().MaxFatigue = 100;
        e.GetOrCreateComponent<FatigueComponent>().FatigueThreshold = 80;
        e.GetOrCreateComponent<FatigueComponent>().FatigueSpeed = 1f;
        e.GetOrCreateComponent<FatigueComponent>().DefaultRest = 5f;

        this.GetParent<Map>().el.Add(e);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        this.GetParent<Map>().el.Remove(e);
    }
}
