using System;
using Godot;
using GodotAnalysers;

[SceneReference("TileMapObject.tscn")]
public partial class TileMapObject
{
    protected Context context;

    public class Context
    {
        public Vector2 Position { get; set; }
        public Map.Context MapContext { get; set; }
        public Vector2[] BlockingCells { get; set; } = new Vector2[0];

        public void Move(Vector2 delta)
        {
            this.Position += delta;
            this.MapContext.UpdatePosition(this);
        }

        public void InitPosition(Vector2 position)
        {
            this.Position = this.MapContext.MapToWorld(this.MapContext.WorldToMap(position));
            this.MapContext.AddPosition(this);
        }
    }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.Position = this.context.Position;
    }

    public virtual void InitContext(Map.Context mapContext)
    {
        this.context = this.context ?? new Context();
        this.context.MapContext = this.GetParent<Map>().context;
        this.context.InitPosition(this.Position);
    }
}
