using LocomotorECS;

public class FatigueToSleepUpdateSystem : MatcherEntitySystem
{
    public FatigueToSleepUpdateSystem() : base(new Matcher()
        .All<FatigueComponent>()
        .Exclude<FatigueSleepComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var fatigue = entity.GetComponent<FatigueComponent>();

        if (fatigue.CurrentFatigue > fatigue.MaxFatigue)
        {
            entity.GetOrCreateComponent<FatigueSleepComponent>().Enable();
        }
    }
}
