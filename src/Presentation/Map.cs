using Godot;
using GodotAnalysers;
using System;

[SceneReference("Map.tscn")]
public partial class Map
{
    public MapGraphData graph = new MapGraphData(25, 25);

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }
}
