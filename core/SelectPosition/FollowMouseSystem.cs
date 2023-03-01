using LocomotorECS;

public class FollowMouseSystem : MatcherEntitySystem
{
    public FollowMouseSystem() : base(new Matcher().All<MouseInputComponent>().All<FollowMouseComponent>().All<PositionComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        entity.GetComponent<PositionComponent>().Position = entity.GetComponent<MouseInputComponent>().MousePosition;
    }
}
