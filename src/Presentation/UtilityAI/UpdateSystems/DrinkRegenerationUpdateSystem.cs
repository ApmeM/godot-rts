using LocomotorECS;

public class DrinkRegenerationUpdateSystem : MatcherEntitySystem
{
    public DrinkRegenerationUpdateSystem() : base(new Matcher().All<DrinkRegenerationComponent>().All<DrinkableComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var drinkable = entity.GetComponent<DrinkableComponent>();
        var regeneration = entity.GetComponent<DrinkRegenerationComponent>();

        if (drinkable.CurrentAmount < regeneration.MaxAmount)
        {
            drinkable.CurrentAmount += delta * regeneration.Regeneration;
            if (drinkable.CurrentAmount > regeneration.MaxAmount)
            {
                drinkable.CurrentAmount = regeneration.MaxAmount;
            }
        }
    }
}
