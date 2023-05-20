using Leopotam.EcsLite;

public class FatigueSleepingUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var delta = systems.GetShared<World.SharedData>().delta;
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<FatigueSleepComponent>()
            .Inc<FatigueComponent>()
            .End();

        var fatigues = world.GetPool<FatigueComponent>();
        var sleeps = world.GetPool<FatigueSleepComponent>();

        foreach (var entity in filter)
        {
            ref var fatigue = ref fatigues.Get(entity);
            var sleep = sleeps.Get(entity);

            fatigue.CurrentFatigue -= sleep.RestSpeed * delta;

            if (fatigue.CurrentFatigue <= 0)
            {
                fatigue.CurrentFatigue = 0;
                sleeps.Del(entity);
            }
        }
    }
}
