using LocomotorECS;

public class PositionBindToMapSystem : MatcherEntitySystem
{
    private readonly GameContext gameContext;

    public PositionBindToMapSystem(GameContext gameContext) : base(new Matcher().All<PositionComponent>().All<BindToMapComponent>())
    {
        this.gameContext = gameContext;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);
        var position = entity.GetComponent<PositionComponent>();
        position.Position = this.gameContext.GetCellPosition(position.Position);
    }
}
