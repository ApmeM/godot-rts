using Leopotam.EcsLite;

public class DrinkMoveUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
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

        var thristingEntities = world.Filter()
            .Inc<PersonDecisionDrinkComponent>()
            .Inc<DrinkThristingComponent>()
            .Inc<MovingComponent>()
            .Inc<PositionComponent>()
            .Inc<PlayerComponent>()
            .End();

        var availabilityHolders = world.Filter()
            .Inc<PersonDecisionDrinkComponent>()
            .Inc<DrinkThristingComponent>()
            .Inc<AvailabilityHolderComponent>()
            .End();

        var movings = world.GetPool<MovingComponent>();
        var positions = world.GetPool<PositionComponent>();

        foreach (var thristingEntity in thristingEntities)
        {
            var waterEntity = CommonLambdas.FindClosestAvailableSource(world, waterEntities, thristingEntity, availabilityHolders, true);
            if (waterEntity == -1 ||
                positions.GetAdd(thristingEntity).Position == positions.GetAdd(waterEntity).Position)
            {
                continue;
            }

            movings.GetAdd(thristingEntity).PathTarget = positions.GetAdd(waterEntity).Position;
        }
    }
}
