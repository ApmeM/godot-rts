using Leopotam.EcsLite;

public class NotificationsCleanupUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<NotificationComponent>()
            .End();

        var notifications = world.GetPool<NotificationComponent>();
        foreach (var entity in filter)
        {
            notifications.GetAdd(entity).Notification = Notifications.None;
        }
    }
}
