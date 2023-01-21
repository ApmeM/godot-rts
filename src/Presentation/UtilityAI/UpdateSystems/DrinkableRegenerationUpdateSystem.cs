using LocomotorECS;

public class DrinkableRegenerationUpdateSystem : MatcherEntitySystem
{
    public DrinkableRegenerationUpdateSystem() : base(new Matcher()
        .All<DrinkableRegenerationComponent>()
        .All<DrinkableComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var drinkable = entity.GetComponent<DrinkableComponent>();
        var regeneration = entity.GetComponent<DrinkableRegenerationComponent>();

        if (drinkable.CurrentAmount >= regeneration.MaxAmount)
        {
            return;
        }
        
        drinkable.CurrentAmount += delta * regeneration.Regeneration;
        if (drinkable.CurrentAmount > regeneration.MaxAmount)
        {
            drinkable.CurrentAmount = regeneration.MaxAmount;
        }
    }
}
