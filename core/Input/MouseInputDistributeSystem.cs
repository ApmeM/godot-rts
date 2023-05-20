using System;
using System.Linq.Struct;
using Leopotam.EcsLite;

public class MouseInputDistributeSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var inputEntity = world.Filter()
            .Inc<MouseInputDistributionComponent>()
            .End()
            .Build()
            .Single();

        var mouseInputEntities = world.Filter()
            .Inc<MouseInputComponent>()
            .End();

        var mouseInputs = world.GetPool<MouseInputComponent>();
        var toDistribute = world.GetPool<MouseInputDistributionComponent>().Get(inputEntity);

        foreach (var mouseInputEntity in mouseInputEntities)
        {
            ref var mouseInput = ref mouseInputs.GetAdd(mouseInputEntity);

            mouseInput.MousePosition = toDistribute.MousePosition;
            mouseInput.MouseButtons = toDistribute.MouseButtons;
            mouseInput.JustPressedButtins = toDistribute.JustPressedButtins;
            mouseInput.JustReleasedButtins = toDistribute.JustReleasedButtins;
        }
    }
}
