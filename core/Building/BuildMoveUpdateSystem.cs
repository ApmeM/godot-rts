using System;
using System.Collections.Generic;
using System.Numerics;
using Leopotam.EcsLite;

public class BuildMoveUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var constructionEntities = world.Filter()
            .Inc<ConstructionComponent>()
            .Inc<PositionComponent>()
            .Inc<PlayerComponent>()
            // Might have Availability
            .End();

        using (var e = constructionEntities.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                return;
            }
        }

        var builderEntities = world.Filter()
            .Inc<PersonDecisionBuildComponent>()
            .Inc<BuilderComponent>()
            .Inc<MovingComponent>()
            .Inc<PositionComponent>()
            .Inc<PlayerComponent>()
            .End();

        var availabilityHolders = world.Filter()
            .Inc<PersonDecisionBuildComponent>()
            .Inc<BuilderComponent>()
            .Inc<AvailabilityHolderComponent>()
            .End();

        var movings = world.GetPool<MovingComponent>();
        var positions = world.GetPool<PositionComponent>();

        foreach (var builderEntity in builderEntities)
        {
            var constructionEntity = CommonLambdas.FindClosestAvailableSource(world, constructionEntities, builderEntity, availabilityHolders, false);
            if (constructionEntity == -1 ||
                positions.GetAdd(builderEntity).Position == positions.GetAdd(constructionEntity).Position)
            {
                continue;
            }

            movings.Get(builderEntity).PathTarget = positions.Get(constructionEntity).Position;
        }
    }
}
