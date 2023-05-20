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

        for (var i = 0; i < 1000; i++)
        {
            var notification = notificationComponent.GetAdd(notificationEntity);
            w.Process(0.1f);
            var count = persons.Build().Count();
            Console.WriteLine($"Step {(i / 10).ToString()}, population = {count.ToString()}, entities count = {w.world.Filter().End().Build().Count()}");
            if (notification.SleepingOnTheGround)
            {
                Console.WriteLine($"    Your people are sleeping on the ground. Build more houses.");
            }
            if (notification.ThristingDead)
            {
                Console.WriteLine($"    Your people are thristing and dying. Build more wells.");
            }
        }
    }
}