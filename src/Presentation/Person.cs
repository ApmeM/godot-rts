using System;
using Godot;
using GodotAnalysers;
using LocomotorECS;

[SceneReference("Person.tscn")]
public partial class Person : EntityTypeNode2DRenderSystem.IEntityNode2D, IMinimapElement
{
    public Entity e { get; set; }

    public bool VisibleOnBorder => false;

    public Texture Texture => this.sprite.Texture;

    public Random r = new Random();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Groups.MinimapElement);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.sprite2.Visible = e.GetComponent<SelectedComponent>()?.Enabled ?? false;
    }
}
