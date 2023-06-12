using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

public class ReproductionUpdateSystem : IEcsRunSystem
{
    private readonly Dictionary<int, int> data = new Dictionary<int, int>();

    public void Run(IEcsSystems systems)
    {
        var r = systems.GetShared<World.SharedData>().random;

        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<FatigueSleepComponent>()
            .Inc<FatigueComponent>()
            .Inc<AvailabilityHolderComponent>()
            .Inc<PlayerComponent>()
            .End();

        var holders = world.GetPool<AvailabilityHolderComponent>();
        var players = world.GetPool<PlayerComponent>();
        var positions = world.GetPool<PositionComponent>();

        data.Clear();
        foreach (var entity in filter)
        {
            var holder = holders.GetAdd(entity);
            if (!data.ContainsKey(holder.OccupiedEntity))
            {
                data[holder.OccupiedEntity] = 0;
            }

            data[holder.OccupiedEntity]++;

            if (data[holder.OccupiedEntity] > 1 && r.NextDouble() < 0.025)
            {
                var player = players.GetAdd(entity);
                var person = Entities.Build(world, EntityTypeComponent.EntityTypes.Person, player.PlayerId);
                positions.GetAdd(person).Position = positions.GetAdd(entity).Position;
            }
        }
    }
}
