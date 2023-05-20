using Godot;
using Leopotam.EcsLite;

public class Node2DPositionRenderSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<Node2DComponent>()
            .Inc<PositionComponent>()
            .End();

        var nodes = world.GetPool<Node2DComponent>();
        var positions = world.GetPool<PositionComponent>();

        foreach (var entity in filter)
        {
            ref var node = ref nodes.GetAdd(entity);
            var position = positions.GetAdd(entity);

            node.Node.Position = new Vector2(position.Position.X, position.Position.Y);
        }
    }
}
