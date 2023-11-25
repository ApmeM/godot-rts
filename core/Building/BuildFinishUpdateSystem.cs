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

        var constructionComplete = false;

        foreach (var entity in filter)
        {
            var construction = constructions.Get(entity);
            if (construction.BuildProgress < construction.MaxBuildProgress)
            {
                continue;
            }

            constructionComplete = true;

            // TODO: add separate systems to handle construction done.
            construction.ConstructionDone?.Invoke(entity);
            construction.ConstructionDone = null;
            constructions.Del(entity);
        }

        if (constructionComplete)
        {
            NotificationUtils.Notify(systems, Notifications.ConstructionComplete);
        }
    }
}
