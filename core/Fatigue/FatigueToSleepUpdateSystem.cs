using LocomotorECS;

public class FatigueToSleepUpdateSystem : MatcherEntitySystem
{
    private readonly Entity notification;

    public FatigueToSleepUpdateSystem(Entity notification) : base(new Matcher()
        .All<FatigueComponent>()
        .Exclude<FatigueSleepComponent>())
    {
        this.notification = notification;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var fatigue = entity.GetComponent<FatigueComponent>();

        if (fatigue.CurrentFatigue > fatigue.MaxFatigue)
        {
            entity.GetComponent<FatigueSleepComponent>().Enable();
            this.notification.GetComponent<NotificationComponent>().SleepingOnTheGround = true;
        }
    }
}
