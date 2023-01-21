using LocomotorECS;

public class FatiguingUpdateSystem : MatcherEntitySystem
{
    public FatiguingUpdateSystem() : base(new Matcher().All<FatigueComponent>().Exclude<SleepComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var fatigue = entity.GetComponent<FatigueComponent>();
        var isBuilding = entity.GetComponent<BuildingComponent>()?.Enabled ?? false;

        fatigue.CurrentFatigue += fatigue.FatigueSpeed * delta * (isBuilding ? 5 : 1);

        if (fatigue.CurrentFatigue > fatigue.MaxFatigue)
        {
            entity.GetComponent<MovingComponent>()?.Disable();
            entity.GetComponent<ThristingComponent>()?.Disable();
            entity.GetOrCreateComponent<SleepComponent>().Enable();
        }
    }
}
