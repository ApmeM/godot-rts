using System;
using System.Numerics;
using Leopotam.EcsLite;

public class WalkingUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var r = systems.GetShared<World.SharedData>().random;
     
        var data = systems.GetShared<World.SharedData>();

        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<PersonDecisionWalkComponent>()
            .Inc<MovingComponent>()
            .Inc<PositionComponent>()
            .Exc<FatigueSleepComponent>()
            .End();

        var movings = world.GetPool<MovingComponent>();
        var positions = world.GetPool<PositionComponent>();

        foreach (var entity in filter)
        {
            ref var moving = ref movings.GetAdd(entity);
            var position = positions.GetAdd(entity);

            if (moving.PathTarget != Vector2Ext.Inf)
            {
                continue;
            }

            moving.PathTarget = position.Position + new Vector2(r.Next((int)data.stepX * 10) - data.stepX * 5, r.Next((int)data.stepY) * 10 - data.stepY * 5);
        }
    }
}
