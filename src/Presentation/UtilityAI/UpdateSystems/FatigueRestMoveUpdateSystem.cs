using System.Linq;
using LocomotorECS;

public class FatigueRestMoveUpdateSystem : MatcherEntitySystem
{
    private MatcherEntityList restSource;

    public FatigueRestMoveUpdateSystem() : base(new Matcher()
        .All<PersonDecisionSleepComponent>()
        .All<PositionComponent>()
        .All<MovingComponent>()
        .Exclude<FatigueSleepComponent>())
    {
    }

    protected override void DoAction(float delta)
    {
        if (!restSource.Entities.Any())
        {
            return;
        }

        base.DoAction(delta);
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var position = entity.GetComponent<PositionComponent>();

        var closestSource = restSource.Entities
                            .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                            .FirstOrDefault();
        var closestRest = closestSource?.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;

        if (position.Position == closestRest)
        {
            entity.GetComponent<FatigueSleepComponent>().Enable();
            return;
        }

        entity.GetComponent<MovingComponent>().PathTarget = closestRest;
    }

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.restSource = new MatcherEntityList(entityList, new Matcher().All<RestComponent>());
        return base.FilterEntityList(entityList);
    }
}
