using System.Collections.Generic;
using Leopotam.EcsLite;

public class Node2DDyingRenderSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<Node2DComponent>()
            .Inc<DeadComponent>()
            .End();

        var nodes = world.GetPool<Node2DComponent>();

        foreach (var entity in filter)
        {
            ref var node = ref nodes.GetAdd(entity);

            node.Node.QueueFree();
            node.Node.GetParent().
            RemoveChild(node.Node);
            node.Node = null;

            nodes.Del(entity);
        }
    }
}
