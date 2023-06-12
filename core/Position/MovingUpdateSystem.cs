using System.Numerics;
using Leopotam.EcsLite;

public class MovingUpdateSystem : IEcsRunSystem
{
    private readonly IPathFinder pathFinder;

    public MovingUpdateSystem(IPathFinder pathFinder)
    {
        this.pathFinder = pathFinder;
    }

    public void Run(IEcsSystems systems)
    {
        var delta = systems.GetShared<World.SharedData>().delta;
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<PositionComponent>()
            .Inc<MovingComponent>()
            .Exc<FatigueSleepComponent>()
            .Exc<DeadComponent>()
            .End();

        var positions = world.GetPool<PositionComponent>();
        var movings = world.GetPool<MovingComponent>();

        foreach (var entity in filter)
        {
            ref var position = ref positions.GetAdd(entity);
            ref var moving = ref movings.GetAdd(entity);

            if (moving.PathTarget == Vector2Ext.Inf)
            {
                continue;
            }

            if (moving.Path.Count == 0 || moving.PathTarget != moving.Path[moving.Path.Count - 1])
            {
                moving.Path.Clear();
                var newPath = this.pathFinder.FindPath(position.Position, moving.PathTarget);
                if (newPath == null)
                {
                    moving.PathTarget = Vector2Ext.Inf;
                    continue;
                }

                foreach (var point in newPath)
                {
                    moving.Path.Add(point);
                }

                moving.PathTarget = moving.Path[moving.Path.Count - 1];
            }

            var path = moving.Path[0] - position.Position;
            var motion = path.Normalized() * moving.MoveSpeed * delta;
            if (path.LengthSquared() > motion.LengthSquared())
            {
                position.Position += motion;
            }
            else
            {
                position.Position = moving.Path[0];
                moving.Path.RemoveAt(0);
                if (moving.Path.Count == 0)
                {
                    moving.PathTarget = Vector2Ext.Inf;
                }
            }
        }
    }
}
