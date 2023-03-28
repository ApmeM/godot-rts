using System.Collections.Generic;
using LocomotorECS;

public class Node2DDyingRenderSystem : MatcherEntitySystem
{
    public Node2DDyingRenderSystem() : base(new Matcher().All<Node2DComponent>().All<DeadComponent>())
    {
    }

    protected override void OnEntityListChanged(HashSet<Entity> added, HashSet<Entity> changed, HashSet<Entity> removed)
    {
        base.OnEntityListChanged(added, changed, removed);

        foreach (var entity in removed)
        {
            var node = entity.GetComponent<Node2DComponent>();
            node.Node.QueueFree();
            node.Node.GetParent().
            RemoveChild(node.Node);
            node.Node = null;
            node.Disable();
        }
    }
}
