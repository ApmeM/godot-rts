using Godot;
using GodotAnalysers;
using System;

[SceneReference("Map.tscn")]
public partial class Map
{
    private MapGraphData graph = new MapGraphData(100, 100);

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public void UpdatePosition(Node2D node, Vector2 newPosition)
    {
        if (!graph.Node2Ds.ContainsKey(node))
        {
            graph.AddNode2D(node, newPosition);
        }
        else
        {
            graph.MoveNode2D(node, newPosition);
        }
    }
}
