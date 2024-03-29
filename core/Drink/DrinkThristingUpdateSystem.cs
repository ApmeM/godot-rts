using System.Collections.Generic;
using LocomotorECS;

public class DrinkThristingUpdateSystem : MatcherEntitySystem
{
    public DrinkThristingUpdateSystem() : base(new Matcher()
        .All<DrinkThristingComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        entity.GetComponent<DrinkThristingComponent>().CurrentThristing -= entity.GetComponent<DrinkThristingComponent>().ThristSpeed * delta;
    }
}
