using Leopotam.EcsLite;

public class FatigueMoveUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var restEntities = world.Filter()
            .Inc<RestComponent>()
            .Inc<PositionComponent>()
            .Inc<PlayerComponent>()
            // Might have Availability
            .End();

        using (var e = restEntities.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                return;
            }
        }

        var filter = world.Filter()
            .Inc<PersonDecisionSleepComponent>()
            .Inc<FatigueComponent>()
            .Inc<MovingComponent>()
            .Inc<PositionComponent>()
            .Inc<PlayerComponent>()
            .End();

        var availabilityHolders = world.Filter()
            .Inc<FatigueSleepComponent>()
            .Inc<FatigueComponent>()
            .Inc<AvailabilityHolderComponent>()
            .End();

        var rests = world.GetPool<RestComponent>();
        var movings = world.GetPool<MovingComponent>();
        var positions = world.GetPool<PositionComponent>();
        var sleeps = world.GetPool<FatigueSleepComponent>();

        foreach (var entity in filter)
        {
            var restEntity = CommonLambdas.FindClosestAvailableSource(world, restEntities, entity, availabilityHolders, false);
            if (restEntity == -1)
            {
                continue;
            }

            if (positions.Get(entity).Position == positions.Get(restEntity).Position)
            {
                sleeps.Add(entity).RestSpeed = rests.Get(restEntity).Regeneration;
                sleeps.Get(entity).InHouse = true;
                continue;
            }

            movings.Get(entity).PathTarget = positions.Get(restEntity).Position;
        }
    }
}