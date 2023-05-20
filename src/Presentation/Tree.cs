using Godot;
using GodotAnalysers;
using Leopotam.EcsLite;

[SceneReference("Tree.tscn")]
public partial class Tree : EntityTypeNode2DRenderSystem.IEntityNode2D, IMinimapElement
{
    public int e { get; set; }
    public EcsWorld world { get; set; }

    public EntityTypeComponent.EntityTypes EntityType => EntityTypeComponent.EntityTypes.Tree;

    [Export]
    public int PlayerId { get; set; }

    public bool VisibleOnBorder => false;

    public Texture Texture => this.sprite.Texture;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Groups.MinimapElement);
    }
}
