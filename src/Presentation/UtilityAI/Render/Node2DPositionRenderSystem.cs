using System.Collections.Generic;
using LocomotorECS;

public class Node2DPositionRenderSystem : MatcherEntitySystem
{
    public Node2DPositionRenderSystem() : base(new Matcher().All<Node2DComponent>().All<PositionComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var node = entity.GetComponent<Node2DComponent>();
        var position = entity.GetComponent<PositionComponent>();
        
        node.Node.Position = position.Position;
    }
}
