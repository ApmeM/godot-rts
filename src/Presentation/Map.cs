using GodotAnalysers;
using Godot;

[SceneReference("Map.tscn")]
public partial class Map
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }
}
