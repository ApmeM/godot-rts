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

        var notificationEntities = world.Filter()
            .Inc<NotificationComponent>()
            .End();

        var fatigues = world.GetPool<FatigueComponent>();
        var fatigueSleepigs = world.GetPool<FatigueSleepComponent>();
        var notifications = world.GetPool<NotificationComponent>();

        var notified = false;

        foreach (var entity in filter)
        {
            var fatigue = fatigues.Get(entity);
            if (fatigue.CurrentFatigue <= fatigue.MaxFatigue)
            {
                continue;
            }

            fatigueSleepigs.Add(entity).RestSpeed = fatigues.Get(entity).DefaultRest;
            fatigueSleepigs.Get(entity).InHouse = false;
            if (!notified)
            {
                notified = true;
                foreach (var notificationEntity in notificationEntities)
                {
                    notifications.GetAdd(notificationEntity).SleepingOnTheGround = true;
                }
            }
        }
    }
}
