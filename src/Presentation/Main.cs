using Godot;
using GodotAnalysers;
using GodotRts.Achievements;
using GodotRts.Presentation.Utils;

[SceneReference("Main.tscn")]
public partial class Main
{
    public World world;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        // For debug purposes all achievements can be reset
        this.di.localAchievementRepository.ResetAchievements();

        this.world = new World(this.map);
        this.world.BuildFromDesignTime();
        this.map.ClearChildren();
        this.world.BuildFence(30, this.map.CellSize.x, this.map.CellSize.y);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        this.world.Process(delta);
    }
}
