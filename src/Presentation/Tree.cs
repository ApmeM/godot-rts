using Godot;
using GodotAnalysers;

[SceneReference("Tree.tscn")]
public partial class Tree
{
    public class Context : TileMapObject.Context
    {
    }

    private Context myContext => (Context)this.context;


    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void InitContext(Map.Context mapContext)
    {
        this.context = this.context ?? new Context();
        this.context.BlockingCells = new Vector2[] { Vector2.Zero };
        base.InitContext(mapContext);
    }
}
