using Godot;
using GodotAnalysers;

[SceneReference("SelectedActions.tscn")]
public partial class SelectedActions
{
    public World World { get; set; }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }
}
