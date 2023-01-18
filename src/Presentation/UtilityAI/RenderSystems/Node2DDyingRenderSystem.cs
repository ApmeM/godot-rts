using LocomotorECS;

public class Node2DDyingRenderSystem : MatcherEntitySystem
{
    public Node2DDyingRenderSystem() : base(new Matcher().All<Node2DComponent>().All<DyingComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var node = entity.GetComponent<Node2DComponent>();
        var dying = entity.GetComponent<DyingComponent>();
        
        if(dying.IsDead)
        {
            node.Node.QueueFree();
            node.Node.GetParent().RemoveChild(node.Node);
        }
    }
}
