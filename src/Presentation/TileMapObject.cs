using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;

[SceneReference("TileMapObject.tscn")]
public partial class TileMapObject
{
    [Export]
    public NodePath TileMapReference;

    protected TileMap tileMap;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.tileMap = this.GetNode<TileMap>(this.TileMapReference);

        this.Position = this.tileMap.ArrangeGlobalPositionToCell(this.Position);
    }
}
