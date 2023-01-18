using LocomotorECS;

public class ThristingUpdateSystem : MatcherEntitySystem
{
    public ThristingUpdateSystem() : base(new Matcher().All<ThristingComponent>().All<DyingComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var thristing = entity.GetComponent<ThristingComponent>();

        thristing.CurrentThristLevel -= thristing.ThristSpeed * delta;

        if (thristing.CurrentThristLevel < 0)
        {
            var dying = entity.GetComponent<DyingComponent>();
            dying.IsDead = true;
        }
    }
}
