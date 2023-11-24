using Leopotam.EcsLite;

public class BuildFinishUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<ConstructionComponent>()
            .End();

        var constructions = world.GetPool<ConstructionComponent>();

        foreach (var entity in filter)
        {
            var construction = constructions.Get(entity);
            if (construction.BuildProgress < construction.MaxBuildProgress)
            {
                continue;
            }
            
            // TODO: add separate systems to handle construction done.
            construction.ConstructionDone?.Invoke(entity);
            construction.ConstructionDone = null;
            constructions.Del(entity);
        }
    }
}