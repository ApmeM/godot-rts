using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Struct;
using System.Numerics;

public class Program
{
    public static void Main(string[] args)
    {
        var w = new World(a => a, a => a, 1, 1);
        w.Init();
        w.BuildForTest(1, 1);

        int notificationEntity;
        using (var notifications = w.world.Filter().Inc<NotificationComponent>().End().GetEnumerator())
        {
            notifications.MoveNext();
            notificationEntity = notifications.Current;
        }
        var notificationComponent = w.world.GetPool<NotificationComponent>();

        var persons = w.world.Filter().Inc<PersonComponent>().End();

        float totalTime = 0;

        for (var i = 0; i < 3000; i++)
        {
            totalTime += 0.1f;
            var notification = notificationComponent.GetAdd(notificationEntity);
            w.Process(0.1f);
            var count = persons.Build().Count();
            Console.WriteLine($"Step {(int)totalTime}, population = {count}, entities count = {w.world.Filter().End().Build().Count()}");
            if (notification.Notification != Notifications.None)
            {
                Console.WriteLine($"    {notification.Notification.ToReadableText()}");
            }
        }
    }
}