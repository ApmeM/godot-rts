using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;

[SceneReference("TileMapObject.tscn")]
public partial class TileMapObject
{
    protected Map map;

    private Vector2 lastCell;

    protected Vector2[] BlockingCells { get; set; } = new Vector2[0];

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.map = this.GetParent<Map>();

        this.lastCell = this.map.GlobalToMap(this.Position);
        this.Position = this.map.MapToGlobal(lastCell);
        map.graph.AddNode2D(this, this.lastCell, this.BlockingCells);
        GD.Print($"Adding {GetType()} to the graph at {lastCell}");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        var currentCell = this.map.GlobalToMap(this.Position);
        if (lastCell != currentCell)
        {
            lastCell = currentCell;
            if (!map.graph.Node2Ds.ContainsKey(this))
            {
                map.graph.AddNode2D(this, this.lastCell, this.BlockingCells);
            }
            else
            {
                map.graph.MoveNode2D(this, this.lastCell, this.BlockingCells);
            }
        }
    }
}
