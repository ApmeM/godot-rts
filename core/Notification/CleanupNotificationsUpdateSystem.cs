using LocomotorECS;

public class CleanupNotificationsUpdateSystem : MatcherEntitySystem
{
    public CleanupNotificationsUpdateSystem() : base(new Matcher()
        .All<NotificationComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        entity.GetComponent<NotificationComponent>().ThristingDead = false;
        entity.GetComponent<NotificationComponent>().SleepingOnTheGround = false;
    }
}
