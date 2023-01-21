using LocomotorECS;

public class FatigueSleepNoThristingSyncUpdateSystem : MatcherEntitySystem
{
    public FatigueSleepNoThristingSyncUpdateSystem() : base(new Matcher()
        .All<FatigueSleepComponent>()
        .All<DrinkThristingComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        entity.GetComponent<DrinkThristingComponent>().Disable();
    }
}
