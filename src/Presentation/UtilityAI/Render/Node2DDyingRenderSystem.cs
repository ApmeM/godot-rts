using LocomotorECS;

public class Node2DDyingRenderSystem : MatcherEntitySystem
{
    public Node2DDyingRenderSystem() : base(new Matcher().All<Node2DComponent>().All<DeadComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var node = entity.GetComponent<Node2DComponent>();
        node.Node.QueueFree();
        node.Node.GetParent().RemoveChild(node.Node);
        entity.Disable();
    }
}
