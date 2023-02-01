using System;
using Godot;
using GodotAnalysers;
using LocomotorECS;

[SceneReference("Person.tscn")]
public partial class Person : EntityTypeNode2DRenderSystem.IEntityNode2D
{
    public Entity e { get; set; }

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
           "Fatigue: " + this.e.GetComponent<FatigueComponent>().CurrentFatigue.ToString("#") + "\n" +
           ((this.e.GetComponent<SelectedComponent>()?.Enabled ?? false) ? "Selected" : "") + "\n"
           ;
    }
}
