using System.Linq;
using LocomotorECS;

public class FatigueMoveUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> restSources;

    public FatigueMoveUpdateSystem(EntityLookup<int> restSources) : base(new Matcher()
        .All<PersonDecisionSleepComponent>()
        .All<PositionComponent>()
        .All<MovingComponent>()
        .All<PlayerComponent>()
        .Exclude<FatigueSleepComponent>())
    {
        this.restSources = restSources;
    }

    protected override void DoAction(float delta)
    {
        if (!restSources.Any(a => a.Value.Entities.Any()))
        {
            return;
        }

        base.DoAction(delta);
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var position = entity.GetComponent<PositionComponent>();
        var player = entity.GetComponent<PlayerComponent>();

        var closestSource = restSources[player.PlayerId].Entities
                            .Where(a => a.GetComponent<AvailabilityComponent>()?.IsAvailable(entity) ?? true)
                            .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                            .FirstOrDefault();
        var closestRest = closestSource?.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;

        if (position.Position == closestRest)
        {
            entity.GetOrCreateComponent<FatigueSleepComponent>().Enable();
            return;
        }

        entity.GetComponent<MovingComponent>().PathTarget = closestRest;
    }
}
