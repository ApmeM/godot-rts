using System.Linq;
using LocomotorECS;

public class SleepingUpdateSystem : MatcherEntitySystem
{
    private MatcherEntityList restSource;

    public SleepingUpdateSystem() : base(new Matcher().All<SleepComponent>().All<FatigueComponent>().All<PositionComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var fatigue = entity.GetComponent<FatigueComponent>();
        var position = entity.GetComponent<PositionComponent>();

        var closestSource = restSource.Entities
                            .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                            .FirstOrDefault();
        var closestRest = closestSource?.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;

        if (position.Position != closestRest || closestRest == Godot.Vector2.Inf)
        {
            fatigue.CurrentFatigue -= fatigue.DefaultRest * delta;
        }
        else
        {
            var rest = closestSource.GetComponent<RestComponent>();
            fatigue.CurrentFatigue -= rest.Regeneration;

            if (fatigue.CurrentFatigue <= 0)
            {
                fatigue.CurrentFatigue = 0;
                entity.GetComponent<SleepComponent>().Disable();
                entity.GetComponent<ThristingComponent>()?.Enable();
                entity.GetComponent<MovingComponent>().Enable();
                return;
            }
        }
    }

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.restSource = new MatcherEntityList(entityList, new Matcher().All<RestComponent>());
        return base.FilterEntityList(entityList);
    }
}
