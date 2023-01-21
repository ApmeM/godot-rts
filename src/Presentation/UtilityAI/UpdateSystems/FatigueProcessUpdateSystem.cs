using LocomotorECS;

public class FatigueProcessUpdateSystem : MatcherEntitySystem
{
    public FatigueProcessUpdateSystem() : base(new Matcher()
        .All<FatigueComponent>()
        .Exclude<FatigueSleepComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var fatigue = entity.GetComponent<FatigueComponent>();
        var isBuilding = entity.GetComponent<BuildingComponent>()?.Enabled ?? false;

        fatigue.CurrentFatigue += fatigue.FatigueSpeed * delta * (isBuilding ? 5 : 1);
    }
}
