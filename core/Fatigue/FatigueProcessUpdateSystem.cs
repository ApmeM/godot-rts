using Leopotam.EcsLite;

public class FatigueProcessUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var delta = systems.GetShared<World.SharedData>().delta;
        
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<FatigueComponent>()
            .Exc<FatigueSleepComponent>()
            .End();

        var fatigues = world.GetPool<FatigueComponent>();
        var decisionBuilds = world.GetPool<PersonDecisionBuildComponent>();
        var holders = world.GetPool<AvailabilityHolderComponent>();

        foreach (var entity in filter)
        {
            ref var fatigue = ref fatigues.GetAdd(entity);

            var isBuilding = decisionBuilds.Has(entity) && holders.Has(entity);

            fatigue.CurrentFatigue += delta * (isBuilding ? fatigue.FatigueBuilderSpeed : fatigue.FatigueSpeed);
        }
    }
}
