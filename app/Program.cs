using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Struct;
using System.Numerics;

public class Program
{
    public static void Main(string[] args)
    {
        var w = new World(a => a, a => a);
        w.BuildForTest(1, 1);

        var notification = w.el.Entities.Select(a => a.GetComponent<NotificationComponent>()).Where(a => a != null).First();
        var persons = w.el.FindEntitiesByTag((int)EntityTypeComponent.EntityTypes.Person);
        for (var i = 0; i < 20000; i++)
        {
            w.Process(0.1f);
            var count = persons.Count;
            Console.WriteLine($"Step {i.ToString()}, population = {count.ToString()}, totalEntities = {w.el.Entities.Count.ToString()}");
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