using Leopotam.EcsLite;

public class DrinkThristingUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var delta = systems.GetShared<World.SharedData>().delta;

        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<DrinkThristingComponent>()
            .Exc<FatigueSleepComponent>()
            .End();

        var thristings = world.GetPool<DrinkThristingComponent>();

        foreach (var entity in filter)
        {
            ref var thristing = ref thristings.GetAdd(entity);
            thristing.CurrentThristing -= thristing.ThristSpeed * delta;
        }
    }
}
