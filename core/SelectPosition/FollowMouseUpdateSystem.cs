using Leopotam.EcsLite;

public class FollowMouseUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<FollowMouseComponent>()
            .Inc<MouseInputComponent>()
            .Inc<PositionComponent>()
            .End();

        var mouseInputs = world.GetPool<MouseInputComponent>();
        var positions = world.GetPool<PositionComponent>();

        foreach (var entity in filter)
        {
            positions.Get(entity).Position = mouseInputs.Get(entity).MousePosition;
        }
    }
}
