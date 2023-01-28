using System.Linq;
using LocomotorECS;

public class FatigueSleepingUpdateSystem : MatcherEntitySystem
{
    private MatcherEntityList restSource;

    public FatigueSleepingUpdateSystem() : base(new Matcher()
        .All<FatigueSleepComponent>()
        .All<FatigueComponent>()
        .All<PositionComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var fatigue = entity.GetComponent<FatigueComponent>();
        var position = entity.GetComponent<PositionComponent>();

        var closestSource = restSource.Entities
                            .Where(a => a.GetComponent<AvailabilityComponent>()?.IsAvailable(entity) ?? true)
                            .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                            .FirstOrDefault();
        var closestRest = closestSource?.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;

        if (position.Position != closestRest || closestRest == Godot.Vector2.Inf)
        {
            fatigue.CurrentFatigue -= fatigue.DefaultRest * delta;
            entity.GetComponent<PersonDecisionSleepComponent>().SelectedHouse?.GetComponent<AvailabilityComponent>().CurrentUsers.Remove(entity);
            entity.GetComponent<PersonDecisionSleepComponent>().SelectedHouse = null;
        }
        else
        {
            var rest = closestSource.GetComponent<RestComponent>();
            fatigue.CurrentFatigue -= rest.Regeneration * delta;
            closestSource?.GetComponent<AvailabilityComponent>()?.CurrentUsers.Add(entity);
        }

        if (fatigue.CurrentFatigue <= 0)
        {
            closestSource?.GetComponent<AvailabilityComponent>()?.CurrentUsers.Remove(entity);
            fatigue.CurrentFatigue = 0;
            entity.GetOrCreateComponent<FatigueSleepComponent>().Disable();
            return;
        }
    }

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.restSource = new MatcherEntityList(entityList, new Matcher().All<RestComponent>());
        return base.FilterEntityList(entityList);
    }
}
