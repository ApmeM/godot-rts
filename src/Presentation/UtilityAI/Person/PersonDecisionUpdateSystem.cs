using System.Linq;
using LocomotorECS;

public class PersonDecisionUpdateSystem : MatcherEntitySystem
{
    private MatcherEntityList waterSources;
    private MatcherEntityList buildSources;
    private MatcherEntityList restSources;

    public PersonDecisionUpdateSystem() : base(new Matcher()
            .All<PersonComponent>()
            .Exclude<FatigueSleepComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var position = entity.GetComponent<PositionComponent>();

        entity.GetOrCreateComponent<PersonDecisionDrinkComponent>();
        entity.GetOrCreateComponent<PersonDecisionSleepComponent>();
        entity.GetOrCreateComponent<PersonDecisionBuildComponent>();
        entity.GetOrCreateComponent<PersonDecisionWalkComponent>();

        var thristing = entity.GetComponent<DrinkThristingComponent>();
        if (thristing != null && (
            thristing.CurrentThristing < thristing.ThristThreshold ||
            thristing.CurrentThristing < thristing.MaxThristLevel && entity.GetComponent<PersonDecisionDrinkComponent>().Enabled
        ) && waterSources.Entities.Where(a => a.GetComponent<AvailabilityComponent>().IsAvailable(entity)).Any())
        {
            entity.GetComponent<PrintComponent>().Text = "Drink";
            this.SetDecision<PersonDecisionDrinkComponent>(entity);
            return;
        }

        var fatigue = entity.GetComponent<FatigueComponent>();
        if (fatigue != null && fatigue.CurrentFatigue > fatigue.FatigueThreshold &&
            restSources.Entities.Where(a => a.GetComponent<AvailabilityComponent>().IsAvailable(entity)).Any())
        {
            entity.GetComponent<PrintComponent>().Text = "Sleep";
            this.SetDecision<PersonDecisionSleepComponent>(entity);
            return;
        }

        var biulder = entity.GetComponent<BuilderComponent>();
        if (biulder != null && buildSources.Entities.Where(a => a.GetComponent<AvailabilityComponent>().IsAvailable(entity)).Any())
        {
            entity.GetComponent<PrintComponent>().Text = "Build";
            this.SetDecision<PersonDecisionBuildComponent>(entity);
            return;
        }

        this.SetDecision<PersonDecisionWalkComponent>(entity);
        entity.GetComponent<PrintComponent>().Text = "Walk";
    }

    private void SetDecision<T>(Entity entity)
    {
        CheckDecision<T, PersonDecisionDrinkComponent>(entity);
        CheckDecision<T, PersonDecisionSleepComponent>(entity);
        CheckDecision<T, PersonDecisionBuildComponent>(entity);
        CheckDecision<T, PersonDecisionWalkComponent>(entity);
    }


    private void CheckDecision<T1, T2>(Entity entity) where T2 : Component
    {
        if (typeof(T1) != typeof(T2))
        {
            entity.GetComponent<T2>().Disable();
        }
        else
        {
            entity.GetComponent<T2>().Enable();
        }
    }

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.waterSources = new MatcherEntityList(entityList, new Matcher().All<DrinkableComponent>().All<PositionComponent>());
        this.buildSources = new MatcherEntityList(entityList, new Matcher().All<ConstructionComponent>().All<PositionComponent>());
        this.restSources = new MatcherEntityList(entityList, new Matcher().All<RestComponent>().All<PositionComponent>());
        return base.FilterEntityList(entityList);
    }
}
