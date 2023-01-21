using System.Linq;
using LocomotorECS;

public class BuildMoveUpdateSystem : MatcherEntitySystem
{
    private MatcherEntityList constructionSource;

    public BuildMoveUpdateSystem() : base(new Matcher()
        .All<PersonDecisionBuildComponent>()
        .All<MovingComponent>()
        .All<PositionComponent>()
        .Exclude<FatigueSleepComponent>())
    {
    }

    protected override void DoAction(float delta)
    {
        if (!constructionSource.Entities.Any())
        {
            return;
        }

        base.DoAction(delta);
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var position = entity.GetComponent<PositionComponent>();

        var closestSource = constructionSource.Entities
                            .Where(a => a.GetComponent<ConstructionComponent>().CurrentBuilders.Contains(entity) || a.GetComponent<ConstructionComponent>().CurrentBuilders.Count < a.GetComponent<ConstructionComponent>().MaxNumberOfBuilders)
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
        this.constructionSource = new MatcherEntityList(entityList, new Matcher().All<ConstructionComponent>());
        return base.FilterEntityList(entityList);
    }
}
