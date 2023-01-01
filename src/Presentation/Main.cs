using Godot;
using GodotAnalysers;
using GodotRts.Achievements;

[SceneReference("Main.tscn")]
public partial class Main
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        // For debug purposes all achievements can be reset
        this.di.localAchievementRepository.ResetAchievements();

        this.map.InitContext(25, 25);
    }
}
