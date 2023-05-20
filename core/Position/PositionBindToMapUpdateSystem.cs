using System.Linq.Struct;
using Leopotam.EcsLite;

public class PositionBindToMapUpdateSystem : IEcsRunSystem
{
    private readonly IMazeBuilder gameContext;

    public PositionBindToMapUpdateSystem(IMazeBuilder gameContext)
    {
        this.gameContext = gameContext;
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<PositionComponent>()
            .Inc<BindToMapComponent>()
            .End();

        var positions = world.GetPool<PositionComponent>();

        foreach (var entity in filter)
        {
            ref var position = ref positions.Get(entity);
            position.Position = this.gameContext.GetCellPosition(position.Position);
        }
    }
}
