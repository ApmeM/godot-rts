using System.Linq;
using LocomotorECS;

public class PersonDecisionUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> waterSources;
    private EntityLookup<int> constructionSource;
    private EntityLookup<int> restSources;

    public PersonDecisionUpdateSystem() : base(new Matcher()
            .All<PersonComponent>()
            .All<PlayerComponent>()
            .Exclude<FatigueSleepComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var player = entity.GetComponent<PlayerComponent>();
        var position = entity.GetComponent<PositionComponent>();

        entity.GetOrCreateComponent<PersonDecisionDrinkComponent>();
        entity.GetOrCreateComponent<PersonDecisionSleepComponent>();
        entity.GetOrCreateComponent<PersonDecisionBuildComponent>();
        entity.GetOrCreateComponent<PersonDecisionWalkComponent>();

        var thristing = entity.GetComponent<DrinkThristingComponent>();
        if (thristing != null && (
            thristing.CurrentThristing < thristing.ThristThreshold ||
            thristing.CurrentThristing < thristing.MaxThristLevel && entity.GetComponent<PersonDecisionDrinkComponent>().Enabled
        ) && waterSources[0].Entities.Union(waterSources[player.PlayerId].Entities).Where(a => a.GetComponent<AvailabilityComponent>().IsAvailable(entity)).Any())
        {
            entity.GetComponent<PrintComponent>().Text = "Drink";
            this.SetDecision<PersonDecisionDrinkComponent>(entity);
            return;
        }

        var fatigue = entity.GetComponent<FatigueComponent>();
        if (fatigue != null && fatigue.CurrentFatigue > fatigue.FatigueThreshold &&
            restSources[player.PlayerId].Entities.Where(a => a.GetComponent<AvailabilityComponent>().IsAvailable(entity)).Any())
        {
            entity.GetComponent<PrintComponent>().Text = "Sleep";
            this.SetDecision<PersonDecisionSleepComponent>(entity);
            return;
        }

        var builder = entity.GetComponent<BuilderComponent>();
        if (builder != null && constructionSource[player.PlayerId].Entities.Where(a => a.GetComponent<AvailabilityComponent>().IsAvailable(entity)).Any())
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
        this.waterSources = new EntityLookup<int>(
            new MatcherEntityList(entityList, new Matcher().All<DrinkableComponent>().All<PositionComponent>()),
            e => e.GetComponent<PlayerComponent>()?.PlayerId ?? 0
        );
        this.constructionSource = new EntityLookup<int>(
            new MatcherEntityList(entityList, new Matcher().All<ConstructionComponent>().All<PositionComponent>()),
            e => e.GetComponent<PlayerComponent>()?.PlayerId ?? 0
        );
        this.restSources = new EntityLookup<int>(
            new MatcherEntityList(entityList, new Matcher().All<RestComponent>().All<PositionComponent>()),
            e => e.GetComponent<PlayerComponent>()?.PlayerId ?? 0
        );
        
        return base.FilterEntityList(entityList);
    }
}
