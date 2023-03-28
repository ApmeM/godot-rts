using LocomotorECS;

public class DrinkThristingDeathUpdateSystem : MatcherEntitySystem
{
    private readonly Entity notification;

    public DrinkThristingDeathUpdateSystem(Entity notification) : base(new Matcher()
        .All<DrinkThristingComponent>()
        .Exclude<DeadComponent>())
    {
        this.notification = notification;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        if (entity.GetComponent<DrinkThristingComponent>().CurrentThristing < 0)
        {
            entity.GetComponent<DeadComponent>().Enable();
            notification.GetComponent<NotificationComponent>().ThristingDead = true;
        }
    }
}
