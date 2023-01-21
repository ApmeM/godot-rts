using System.Linq;
using LocomotorECS;

public class DrinkMoveUpdateSystem : MatcherEntitySystem
{
    private MatcherEntityList drinkSource;

    public DrinkMoveUpdateSystem() : base(new Matcher()
        .All<PersonDecisionDrinkComponent>()
        .All<MovingComponent>()
        .All<PositionComponent>()
        .Exclude<FatigueSleepComponent>())
    {
    }

    protected override void DoAction(float delta)
    {
        if (!drinkSource.Entities.Any())
        {
            return;
        }

        base.DoAction(delta);
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var position = entity.GetComponent<PositionComponent>();

        var closestSource = drinkSource.Entities
                            .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                            .FirstOrDefault();

        var closestWater = closestSource?.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;
        if (position.Position == closestWater)
        {
            return;
        }

        entity.GetComponent<MovingComponent>().PathTarget = closestWater;
    }

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.drinkSource = new MatcherEntityList(entityList, new Matcher().All<DrinkableComponent>());
        return base.FilterEntityList(entityList);
    }
}
