using LocomotorECS;

public class BuildingNoBuildSyncUpdateSystem : MatcherEntitySystem
{
    public BuildingNoBuildSyncUpdateSystem() : base(new Matcher()
        .All<BuildingComponent>()
        .Exclude<PersonDecisionBuildComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var building = entity.GetOrCreateComponent<BuildingComponent>();
        building.SelectedConstruction?.GetComponent<AvailabilityComponent>()?.CurrentBuilders.Remove(entity);
        building.SelectedConstruction = null;
        building.Disable();
    }
}
