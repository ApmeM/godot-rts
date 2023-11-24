using System;
using Godot;
using GodotAnalysers;
using Leopotam.EcsLite;

[SceneReference("Person.tscn")]
public partial class Person : EntityTypeNode2DRenderSystem.IEntityNode2D, IMinimapElement
{
    public int e { get; set; }
    public EcsWorld world { get; set; }

    public EntityTypeComponent.EntityTypes EntityType => EntityTypeComponent.EntityTypes.Person;

    [Export]
    public int PlayerId { get; set; }

    public bool VisibleOnBorder => false;

    public Sprite Sprite => this.sprite;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Groups.MinimapElement);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.sprite2.Visible = this.world.GetPool<SelectedComponent>().Has(e);
    }
}
