using System.Linq;
using LocomotorECS;

public class BuildMoveUpdateSystem : MatcherEntitySystem
{
    private EntityGroups<int> constructionSource;

    public BuildMoveUpdateSystem() : base(new Matcher()
        .All<PersonDecisionBuildComponent>()
        .All<MovingComponent>()
        .All<PositionComponent>()
        .All<PlayerComponent>()
        .Exclude<FatigueSleepComponent>())
    {
    }

    protected override void DoAction(float delta)
    {
        if (!constructionSource.Any(a => a.Value.Entities.Any()))
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

        var closestSource = constructionSource[player.PlayerId].Entities
                            .Where(a => a.GetComponent<AvailabilityComponent>()?.IsAvailable(entity) ?? true)
                            .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                            .FirstOrDefault();

        var closestConstruction = closestSource?.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;
        if (position.Position == closestConstruction)
        {
            return;
        }

        entity.GetComponent<MovingComponent>().PathTarget = closestConstruction;
    }

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.constructionSource = new EntityGroups<int>(
            new MatcherEntityList(entityList, new Matcher().All<ConstructionComponent>().All<PositionComponent>()),
            e => e.GetComponent<PlayerComponent>()?.PlayerId ?? 0
        );
        return base.FilterEntityList(entityList);
    }
}
