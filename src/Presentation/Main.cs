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
        new LocalAchievementRepository().ResetAchievements();

        // See achievements definitions in gd-achievements/achievements.json
        this.achievementNotifications.UnlockAchievement("MyFirstAchievement");
        this.achievementList.ReloadList();
    }
}
