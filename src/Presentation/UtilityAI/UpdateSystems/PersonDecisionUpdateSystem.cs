using System.Linq;
using LocomotorECS;

public class PersonDecisionUpdateSystem : MatcherEntitySystem
{
    private MatcherEntityList waterSources;
    private MatcherEntityList buildSources;
    private MatcherEntityList restSources;

    public PersonDecisionUpdateSystem() : base(new Matcher()
            .All<PersonComponent>()
            .Exclude<FatigueSleepComponent>()
            .Exclude<PersonDecisionDrinkComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var position = entity.GetComponent<PositionComponent>();

        entity.GetOrCreateComponent<PersonDecisionDrinkComponent>().Disable();
        entity.GetOrCreateComponent<PersonDecisionSleepComponent>().Disable();
        entity.GetOrCreateComponent<PersonDecisionBuildComponent>().Disable();
        entity.GetOrCreateComponent<PersonDecisionWalkComponent>().Disable();

        var thristing = entity.GetComponent<DrinkThristingComponent>();
        if (thristing != null && thristing.CurrentThristing < thristing.ThristThreshold && waterSources.Entities.Any())
        {
            entity.GetComponent<PrintComponent>().Text = "Drink";
            entity.GetOrCreateComponent<PersonDecisionDrinkComponent>().Enable();
            return;
        }

        var fatigue = entity.GetComponent<FatigueComponent>();
        if (fatigue != null && fatigue.CurrentFatigue > fatigue.FatigueThreshold && restSources.Entities.Any() || entity.GetComponent<PersonDecisionSleepComponent>().Enabled)
        {
            entity.GetComponent<PrintComponent>().Text = "Sleep";
            entity.GetOrCreateComponent<PersonDecisionSleepComponent>().Enable();
            return;
        }

        var biulder = entity.GetComponent<BuilderComponent>();
        if (biulder != null && buildSources.Entities.Any() || entity.GetComponent<PersonDecisionBuildComponent>().Enabled)
        {
            entity.GetComponent<PrintComponent>().Text = "Build";
            entity.GetOrCreateComponent<PersonDecisionBuildComponent>().Enable();
            return;
        }

        entity.GetComponent<PrintComponent>().Text = "Walk";
        entity.GetOrCreateComponent<PersonDecisionWalkComponent>().Enable();
    }

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.waterSources = new MatcherEntityList(entityList, new Matcher().All<DrinkableComponent>().All<PositionComponent>());
        this.buildSources = new MatcherEntityList(entityList, new Matcher().All<ConstructionComponent>().All<PositionComponent>());
        this.restSources = new MatcherEntityList(entityList, new Matcher().All<RestComponent>().All<PositionComponent>());
        return base.FilterEntityList(entityList);
    }
}
