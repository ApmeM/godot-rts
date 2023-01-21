using LocomotorECS;

public class FatigueNoSleepThristingSyncUpdateSystem : MatcherEntitySystem
{
    public FatigueNoSleepThristingSyncUpdateSystem() : base(new Matcher()
        .Exclude<FatigueSleepComponent>()
        .Exclude<DrinkThristingComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        entity.GetComponent<DrinkThristingComponent>()?.Enable();
    }
}
