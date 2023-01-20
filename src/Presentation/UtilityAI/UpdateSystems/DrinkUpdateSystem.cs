using System.Linq;
using Godot;
using LocomotorECS;

public class DrinkUpdateSystem : MatcherEntitySystem
{
    private MatcherEntityList drinkSource;

    public DrinkUpdateSystem() : base(new Matcher().All<DrinkComponent>().All<ThristingComponent>().All<PositionComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var thristing = entity.GetComponent<ThristingComponent>();
        var position = entity.GetComponent<PositionComponent>();

        var closestSource = drinkSource.Entities
                            .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                            .FirstOrDefault();

        var closestRest = closestSource?.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;
        if (position.Position != closestRest)
        {
            entity.GetComponent<DrinkComponent>().Disable();
            entity.GetComponent<MovingComponent>()?.Enable();
            return;
        }
        var drinkable = closestSource.GetComponent<DrinkableComponent>();
        var toDrink = Mathf.Min(Mathf.Min(thristing.DrinkSpeed * delta, thristing.MaxThristLevel - thristing.CurrentThristing), drinkable.CurrentAmount);
        drinkable.CurrentAmount -= toDrink;
        thristing.CurrentThristing += toDrink;

        if (thristing.CurrentThristing >= thristing.MaxThristLevel)
        {
            thristing.CurrentThristing = thristing.MaxThristLevel;
            entity.GetComponent<DrinkComponent>().Disable();
            entity.GetComponent<MovingComponent>()?.Enable();
        }
    }

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.drinkSource = new MatcherEntityList(entityList, new Matcher().All<DrinkableComponent>());
        return base.FilterEntityList(entityList);
    }
}
