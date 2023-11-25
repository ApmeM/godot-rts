using Leopotam.EcsLite;

public class FatigueToSleepUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<FatigueComponent>()
            .Exc<FatigueSleepComponent>()
            .Exc<DeadComponent>()
            .End();

        var fatigues = world.GetPool<FatigueComponent>();
        var fatigueSleepigs = world.GetPool<FatigueSleepComponent>();

        var isSleepOnGround = false;

        foreach (var entity in filter)
        {
            var fatigue = fatigues.Get(entity);
            if (fatigue.CurrentFatigue <= fatigue.MaxFatigue)
            {
                continue;
            }

            isSleepOnGround = true;

            fatigueSleepigs.Add(entity).RestSpeed = fatigues.Get(entity).DefaultRest;
            fatigueSleepigs.Get(entity).InHouse = false;
        }
        
        if (isSleepOnGround)
        {
            NotificationUtils.Notify(systems, Notifications.SleepingOnTheGround);
        }
    }
}
