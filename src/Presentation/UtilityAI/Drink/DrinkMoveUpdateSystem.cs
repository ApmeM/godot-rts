using System.Linq;
using LocomotorECS;

public class DrinkMoveUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> waterSources;

    public DrinkMoveUpdateSystem(EntityLookup<int> drinkSource) : base(new Matcher()
        .All<PersonDecisionDrinkComponent>()
        .All<MovingComponent>()
        .All<PositionComponent>()
        .Exclude<FatigueSleepComponent>())
    {
        this.waterSources = drinkSource;
    }

    protected override void DoAction(float delta)
    {
        if (!waterSources.Any(a => a.Value.Entities.Any()))
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

        var closestSource = waterSources[0].Entities.Union(waterSources[player.PlayerId].Entities)
                            .Where(a => a.GetComponent<AvailabilityComponent>()?.IsAvailable(entity) ?? true)
                            .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                            .FirstOrDefault();

        var closestWater = closestSource?.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;
        if (position.Position == closestWater)
        {
            return;
        }

        entity.GetComponent<MovingComponent>().PathTarget = closestWater;
    }
}
