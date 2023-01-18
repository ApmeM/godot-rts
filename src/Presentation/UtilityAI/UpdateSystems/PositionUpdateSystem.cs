using System.Collections.Generic;
using LocomotorECS;

public class PositionUpdateSystem : MatcherEntitySystem
{
    private readonly GameContext gameContext;

    public PositionUpdateSystem(GameContext gameContext) : base(new Matcher().All<PositionComponent>())
    {
        this.gameContext = gameContext;
    }

    protected override void OnEntityListChanged(HashSet<Entity> added, HashSet<Entity> changed, HashSet<Entity> removed)
    {
        base.OnEntityListChanged(added, changed, removed);
        foreach (var entity in added)
        {
            var position = entity.GetComponent<PositionComponent>();
            position.Position = this.gameContext.AddPosition(position);
        }
        
        foreach (var entity in removed)
        {
            var position = entity.GetComponent<PositionComponent>();
            this.gameContext.RemovePosition(position);
        }
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);
        var position = entity.GetComponent<PositionComponent>();

        this.gameContext.UpdatePosition(position);
    }
}
