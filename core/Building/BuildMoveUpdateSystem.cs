using System.Linq;
using System.Numerics;
using LocomotorECS;

public class BuildMoveUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> constructionSource;

    public BuildMoveUpdateSystem(EntityLookup<int> constructionSource) : base(new Matcher()
        .All<PersonDecisionBuildComponent>()
        .All<MovingComponent>()
        .All<PositionComponent>()
        .All<PlayerComponent>()
        .Exclude<FatigueSleepComponent>())
    {
        this.constructionSource = constructionSource;
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

        var closestConstruction = closestSource?.GetComponent<PositionComponent>()?.Position ?? Vector2Ext.Inf;
        if (position.Position == closestConstruction)
        {
            return;
        }

        entity.GetComponent<MovingComponent>().PathTarget = closestConstruction;
    }
}
