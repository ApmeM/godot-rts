using Godot;
using GodotAnalysers;
using LocomotorECS;

[SceneReference("Tree.tscn")]
public partial class Tree : EntityTypeNode2DRenderSystem.IEntityNode2D
{
    public Entity e { get; set; }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }
}
