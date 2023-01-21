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

        entity.GetComponent<BuildingComponent>().Disable();
    }
}
