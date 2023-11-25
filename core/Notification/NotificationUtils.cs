using System;
using Leopotam.EcsLite;

public class NotificationUtils
{
    public static void Notify(IEcsSystems systems, Notifications notification)
    {
        var world = systems.GetWorld();

        var notificationEntities = world.Filter()
            .Inc<NotificationComponent>()
            .End();
        var notifications = world.GetPool<NotificationComponent>();
        foreach (var notificationEntity in notificationEntities)
        {
            notifications.GetAdd(notificationEntity).Notification = notification;
        }
    }

}
