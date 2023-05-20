using Leopotam.EcsLite;

public class DrinkThristingDeathUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var thristingEntities = world.Filter()
            .Inc<DrinkThristingComponent>()
            .Exc<DeadComponent>()
            .End();

        var notificationEntities = world.Filter()
            .Inc<NotificationComponent>()
            .End();

        var thristings = world.GetPool<DrinkThristingComponent>();
        var notifications = world.GetPool<NotificationComponent>();
        var deads = world.GetPool<DeadComponent>();

        var notified = false;

        foreach (var thristingEntity in thristingEntities)
        {
            var thristing = thristings.GetAdd(thristingEntity);

            if (thristing.CurrentThristing >= 0)
            {
                continue;
            }
            
            deads.Add(thristingEntity);

            if (!notified)
            {
                notified = true;
                foreach (var notificationEntity in notificationEntities)
                {
                    notifications.GetAdd(notificationEntity).ThristingDead = true;
                }
            }
        }
    }
}
