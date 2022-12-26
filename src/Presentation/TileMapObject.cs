using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;

[SceneReference("TileMapObject.tscn")]
public partial class TileMapObject
{
    protected Map map;

    private Vector2 lastCell;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.map = this.GetParent<Map>();

        this.Position = this.map.ArrangeGlobalPositionToCell(this.Position);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        var currentCell = this.map.GlobalToMap(this.Position);
        if (lastCell != currentCell)
        {
            lastCell = currentCell;
            map.UpdatePosition(this, lastCell);
        }
    }
}
