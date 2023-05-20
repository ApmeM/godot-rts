using System;
using Leopotam.EcsLite;

public class DrinkProcessUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var delta = systems.GetShared<World.SharedData>().delta;
        var world = systems.GetWorld();

        var waterEntities = world.Filter()
            .Inc<DrinkableComponent>()
            .Inc<PositionComponent>()
            .Inc<PlayerComponent>()
            // Might have Availability
            .End();

        using (var e = waterEntities.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                return;
            }
        }

        var filter = world.Filter()
            .Inc<PersonDecisionDrinkComponent>()
            .Inc<DrinkThristingComponent>()
            .Inc<PositionComponent>()
            .Inc<PlayerComponent>()
            .End();


        var availabilityHolders = world.Filter()
            .Inc<PersonDecisionDrinkComponent>()
            .Inc<DrinkThristingComponent>()
            .Inc<AvailabilityHolderComponent>()
            .End();

        var positions = world.GetPool<PositionComponent>();
        var players = world.GetPool<PlayerComponent>();
        var availabilities = world.GetPool<AvailabilityComponent>();
        var holders = world.GetPool<AvailabilityHolderComponent>();
        var thristings = world.GetPool<DrinkThristingComponent>();
        var drinkables = world.GetPool<DrinkableComponent>();

        foreach (var entity in filter)
        {
            var waterEntity = CommonLambdas.FindClosestAvailableSource(world, waterEntities, entity, availabilityHolders, true);
            if (waterEntity == -1 ||
                positions.GetAdd(entity).Position != positions.GetAdd(waterEntity).Position)
            {
                holders.Del(entity);
                continue;
            }

            holders.GetAdd(entity).OccupiedEntity = waterEntity;

            ref var drinkable = ref drinkables.GetAdd(waterEntity);
            ref var thristing = ref thristings.GetAdd(entity);

            var toDrink = Math.Min(thristing.DrinkSpeed * delta, drinkable.CurrentAmount);
            toDrink = Math.Min(toDrink, thristing.MaxThristLevel - thristing.CurrentThristing);
            
            drinkable.CurrentAmount -= toDrink;
            thristing.CurrentThristing += toDrink;
        }
    }
}
