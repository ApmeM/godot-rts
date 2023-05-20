using Leopotam.EcsLite;

public class RemoveDeadUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<DeadComponent>()
            .End();

        foreach (var entity in filter)
        {
            world.DelEntity(entity);
        }
    }
}
