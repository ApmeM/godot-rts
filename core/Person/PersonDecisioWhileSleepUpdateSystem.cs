using System;
using System.Numerics;
using Leopotam.EcsLite;

public class PersonDecisioWhileSleepUpdateSystem : IEcsRunSystem
{
    private readonly Random r = new Random();

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var builderEntities = world.Filter().Inc<PersonDecisionBuildComponent>().Inc<FatigueSleepComponent>().End();
        var drinkingEntities = world.Filter().Inc<PersonDecisionDrinkComponent>().Inc<FatigueSleepComponent>().End();
        var sleepingEntities = world.Filter().Inc<PersonDecisionSleepComponent>().Inc<FatigueSleepComponent>().End();
        var walkingEntities = world.Filter().Inc<PersonDecisionWalkComponent>().Inc<FatigueSleepComponent>().End();

        var decisionBuilds = world.GetPool<PersonDecisionBuildComponent>();
        var decisionDrinks = world.GetPool<PersonDecisionDrinkComponent>();
        var decisionSleeps = world.GetPool<PersonDecisionSleepComponent>();
        var decisionWalks = world.GetPool<PersonDecisionWalkComponent>();
        var holder = world.GetPool<AvailabilityHolderComponent>();
        var sleep = world.GetPool<FatigueSleepComponent>();

        foreach (var entity in builderEntities)
        {
            decisionBuilds.Del(entity);
            holder.Del(entity);
        }

        foreach (var entity in drinkingEntities)
        {
            decisionDrinks.Del(entity);
            holder.Del(entity);
        }
        foreach (var entity in sleepingEntities)
        {
            decisionSleeps.Del(entity);
            if (!sleep.Get(entity).InHouse)
            {
                holder.Del(entity);
            }
        }
        foreach (var entity in walkingEntities)
        {
            decisionWalks.Del(entity);
            holder.Del(entity);
        }
    }
}
