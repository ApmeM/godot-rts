using LocomotorECS;

public class DrinkThristingDeathUpdateSystem : MatcherEntitySystem
{
    public DrinkThristingDeathUpdateSystem() : base(new Matcher()
        .All<DrinkThristingComponent>()
        .All<DyingComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        if (entity.GetComponent<DrinkThristingComponent>().CurrentThristing < 0)
        {
            entity.GetComponent<DyingComponent>().IsDead = true;
        }
    }
}
