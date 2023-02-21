using System.Linq;
using Godot;
using LocomotorECS;

public class DrinkProcessUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> waterSources;

    public DrinkProcessUpdateSystem(EntityLookup<int> drinkSource) : base(new Matcher()
        .All<PersonDecisionDrinkComponent>()
        .All<DrinkThristingComponent>()
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

        var thristing = entity.GetComponent<DrinkThristingComponent>();
        var position = entity.GetComponent<PositionComponent>();
        var player = entity.GetComponent<PlayerComponent>();

        var closestSource = waterSources[0].Entities.Union(waterSources[player.PlayerId].Entities)
                            .Where(a => a.GetComponent<AvailabilityComponent>()?.IsAvailable(entity) ?? true)
                            .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                            .FirstOrDefault();

        var closestWater = closestSource?.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;
        if (position.Position != closestWater)
        {
            entity.GetComponent<PersonDecisionDrinkComponent>().SelectedSource?.GetComponent<AvailabilityComponent>().CurrentUsers.Remove(entity);
            entity.GetComponent<PersonDecisionDrinkComponent>().SelectedSource = null;
            return;
        }

        closestSource.GetOrCreateComponent<AvailabilityComponent>()?.CurrentUsers.Add(entity);
        entity.GetComponent<PersonDecisionDrinkComponent>().SelectedSource = closestSource;

        var drinkable = closestSource.GetComponent<DrinkableComponent>();
        var toDrink = Mathf.Min(thristing.DrinkSpeed * delta, drinkable.CurrentAmount);
        drinkable.CurrentAmount -= toDrink;
        thristing.CurrentThristing += toDrink;
    }
}
