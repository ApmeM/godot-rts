using Leopotam.EcsLite;

public static class CommonLambdas
{
    public static int FindClosestAvailableSource(EcsWorld world, EcsFilter sources, int entity, EcsFilter holders, bool useNeutral)
    {
        var positions = world.GetPool<PositionComponent>();
        var players = world.GetPool<PlayerComponent>();
        var availability = world.GetPool<AvailabilityComponent>();
        var availabilityHolders = world.GetPool<AvailabilityHolderComponent>();

        var entityPosition = positions.Get(entity);
        var entityPlayer = players.Get(entity);

        int closestSourceEntity = -1;
        var closestDistance = float.MaxValue;

        foreach (var source in sources)
        {
            var sourcePosition = positions.Get(source);
            var sourcePlayer = players.Get(source);

            var holdersCount = 0;
            foreach (var holder in holders)
            {
                if (availabilityHolders.Get(holder).OccupiedEntity == source && holder != entity)
                {
                    holdersCount++;
                }
            }

            if (sourcePlayer.PlayerId != entityPlayer.PlayerId && (sourcePlayer.PlayerId != 0 || !useNeutral) ||
                availability.Has(source) && availability.Get(source).MaxNumberOfUsers <= holdersCount)
            {
                continue;
            }

            var distance = (entityPosition.Position - sourcePosition.Position).LengthSquared();
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSourceEntity = source;
            }
        }
        return closestSourceEntity;
    }
}
