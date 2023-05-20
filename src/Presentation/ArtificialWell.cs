using GodotAnalysers;
using Godot;
using Leopotam.EcsLite;

[SceneReference("ArtificialWell.tscn")]
public partial class ArtificialWell : EntityTypeNode2DRenderSystem.IEntityNode2D
{
    public int e { get; set; }
    public EcsWorld world { get; set; }

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

        var construction = this.world.GetPool<ConstructionComponent>().Has(e);
        if (!construction)
        {
            this.sprite.Hide();
            this.sprite1.Show();
        }
    }
}
