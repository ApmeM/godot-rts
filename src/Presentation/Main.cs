using System.Linq.Struct;
using Godot;
using GodotAnalysers;
using GodotRts.Achievements;
using GodotRts.Presentation.Utils;

[SceneReference("Main.tscn")]
public partial class Main
{
    public World world;

    private float totalTime = 0;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        // For debug purposes all achievements can be reset
        this.di.localAchievementRepository.ResetAchievements();

        this.world = new World(map.WorldToMap, map.MapToWorld, this.map.CellSize.x, this.map.CellSize.y);
        this.world.AddGodotSpecific(this.map);
        this.world.Init();
        // this.world.BuildFromDesignTime(this.map);
        this.world.BuildForTest(this.map.CellSize.x, this.map.CellSize.y);
        this.map.ClearChildren();
        this.selectedDetails.World = this.world;
        this.selectedActions.World = this.world;
    }

    public override void _Process(float delta)
    {
        delta = 0.1f; // To act in a same way as in console application.
        totalTime += delta;
        base._Process(delta);

        int notificationEntity;
        using (var notifications = this.world.world.Filter().Inc<NotificationComponent>().End().GetEnumerator())
        {
            notifications.MoveNext();
            notificationEntity = notifications.Current;
        }
        var notificationComponent = world.world.GetPool<NotificationComponent>();

        var persons = world.world.Filter().Inc<PersonComponent>().End();
        var notification = notificationComponent.GetAdd(notificationEntity);
        world.Process(delta);
        var count = persons.Build().Count();

        this.notification.Text = $"Step {((int)totalTime).ToString()}, population = {count.ToString()}";

        if (notification.SleepingOnTheGround)
        {
            this.floatingTextManager.ShowValue("Your people are sleeping on the ground. Build more houses.");
        }
        if (notification.ThristingDead)
        {
            this.floatingTextManager.ShowValue("Your people are thristing and dying. Build more wells.");
        }
    }
}
