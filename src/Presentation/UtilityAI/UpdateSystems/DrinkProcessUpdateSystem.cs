using System.Linq;
using Godot;
using LocomotorECS;

public class DrinkProcessUpdateSystem : MatcherEntitySystem
{
    private MatcherEntityList drinkSource;

    public DrinkProcessUpdateSystem() : base(new Matcher()
        .All<PersonDecisionDrinkComponent>()
        .All<DrinkThristingComponent>()
        .All<PositionComponent>()
        .Exclude<FatigueSleepComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var thristing = entity.GetComponent<DrinkThristingComponent>();
        var position = entity.GetComponent<PositionComponent>();

        var closestSource = drinkSource.Entities
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

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.drinkSource = new MatcherEntityList(entityList, new Matcher().All<DrinkableComponent>());
        return base.FilterEntityList(entityList);
    }
}
