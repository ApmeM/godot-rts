using Godot;
using GodotAnalysers;
using LocomotorECS;

[SceneReference("Tree.tscn")]
public partial class Tree : EntityTypeNode2DRenderSystem.IEntityNode2D, IMinimapElement
{
    public Entity e { get; set; }

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
