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

        this.world = new World(map.WorldToMap, map.MapToWorld);
        this.world.AddGodotSpecific(this.map);
        // this.world.BuildFromDesignTime(this.map);
        this.world.BuildForTest(this.map.CellSize.x, this.map.CellSize.y);
        this.map.ClearChildren();
        this.selectedDetails.World = this.world;
        this.selectedActions.World = this.world;
    }

    public override void _Process(float delta)
    {
        // delta = 0.1f; // To act in a same way as in console application.
        totalTime += delta;
        base._Process(delta);
        this.world.Process(delta);

        var notification = this.world.el.Entities.Select(a => a.GetComponent<NotificationComponent>()).Where(a => a != null).First();
        var persons = this.world.el.FindEntitiesByTag((int)EntityTypeComponent.EntityTypes.Person);

        this.notification.Text = $"Step {((int)totalTime).ToString()}, population = {persons.Count.ToString()}, totalEntities = {this.world.el.Entities.Count.ToString()}";
        
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
