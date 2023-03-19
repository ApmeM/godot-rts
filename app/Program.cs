using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using HonkPerf.NET.RefLinq;

public class Program
{
    public static void Main(string[] args)
    {
        var w = new World(a => a, a => a);
        w.BuildForTest(1, 1);
        w.BuildFence(60, 1, 1);

        for (var i = 0; i < 2000; i++)
        {
            w.Process(0.1f);
            var count = w.el.FindEntitiesByTag((int)EntityTypeComponent.EntityTypes.Person).Count;
            Console.WriteLine($"Step {i}, population = {count}");
        }
    }
}