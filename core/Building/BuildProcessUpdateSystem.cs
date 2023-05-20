using System;
using System.Linq;
using Leopotam.EcsLite;

public class BuildProcessUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var delta = systems.GetShared<World.SharedData>().delta;

        var world = systems.GetWorld();

        var constructionEntities = world.Filter()
            .Inc<ConstructionComponent>()
            .Inc<PositionComponent>()
            .Inc<PlayerComponent>()
            // Might have Availability
            // Might have HP
            .End();

        using (var e = constructionEntities.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                return;
            }
        }

        foreach (var e in constructionEntities)
        {
            var res = new object[0];
            world.GetComponents(e, ref res);
        }

        var builderEntities = world.Filter()
            .Inc<PersonDecisionBuildComponent>()
            .Inc<BuilderComponent>()
            .Inc<PositionComponent>()
            .Inc<PlayerComponent>()
            .End();


        var availabilityHolders = world.Filter()
            .Inc<PersonDecisionBuildComponent>()
            .Inc<BuilderComponent>()
            .Inc<AvailabilityHolderComponent>()
            .End();

        var positions = world.GetPool<PositionComponent>();
        var players = world.GetPool<PlayerComponent>();
        var availabilities = world.GetPool<AvailabilityComponent>();
        var holders = world.GetPool<AvailabilityHolderComponent>();
        var constructions = world.GetPool<ConstructionComponent>();
        var builders = world.GetPool<BuilderComponent>();
        var hps = world.GetPool<HPComponent>();

        foreach (var builderEntity in builderEntities)
        {
            var constructionEntity = CommonLambdas.FindClosestAvailableSource(world, constructionEntities, builderEntity, availabilityHolders, false);
            if (constructionEntity == -1 ||
                positions.Get(builderEntity).Position != positions.Get(constructionEntity).Position)
            {
                holders.Del(builderEntity);
                continue;
            }

            holders.GetAdd(builderEntity).OccupiedEntity = constructionEntity;

            var builder = builders.Get(builderEntity);

            ref var construction = ref constructions.Get(constructionEntity);

            var buildProgress = Math.Min(delta * builder.BuildSpeed, construction.MaxBuildProgress - construction.BuildProgress);

            construction.BuildProgress += buildProgress;

            if (hps.Has(constructionEntity))
            {
                ref var hp = ref hps.Get(constructionEntity);
                var hpProgress = hp.MaxHP * buildProgress / construction.MaxBuildProgress;

                hp.HP += hpProgress;
                if (hp.HP > hp.MaxHP)
                {
                    hp.HP = hp.MaxHP;
                }
            }
        }
    }
}
