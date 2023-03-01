using System.Linq;
using System.Numerics;
using LocomotorECS;

public class FatigueSleepingUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> restSources;

    public FatigueSleepingUpdateSystem(EntityLookup<int> restSources) : base(new Matcher()
        .All<FatigueSleepComponent>()
        .All<FatigueComponent>()
        .All<PositionComponent>()
        .All<PlayerComponent>())
    {
        this.restSources = restSources;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var fatigue = entity.GetComponent<FatigueComponent>();
        var position = entity.GetComponent<PositionComponent>();
        var player = entity.GetComponent<PlayerComponent>();

        var closestSource = restSources[player.PlayerId].Entities
                            .Where(a => a.GetComponent<AvailabilityComponent>()?.IsAvailable(entity) ?? true)
                            .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                            .FirstOrDefault();
        var closestRest = closestSource?.GetComponent<PositionComponent>()?.Position ?? Vector2Ext.Inf;

        if (position.Position != closestRest || closestRest == Vector2Ext.Inf)
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
}
