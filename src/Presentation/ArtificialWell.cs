using GodotAnalysers;
using Godot;
using LocomotorECS;

[SceneReference("ArtificialWell.tscn")]
public partial class ArtificialWell : EntityTypeNode2DRenderSystem.IEntityNode2D
{
    public Entity e { get; set; }

    public EntityTypeComponent.EntityTypes EntityType => EntityTypeComponent.EntityTypes.ArtificialWell;

    [Export]
    public int PlayerId { get; set; }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        var construction = this.e.GetComponent<ConstructionComponent>();
        if (construction == null)
        {
            this.sprite.Hide();
            this.sprite1.Show();
        }
    }
}
