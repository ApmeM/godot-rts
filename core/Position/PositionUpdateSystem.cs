using Leopotam.EcsLite;

public class PositionUpdateSystem : IEcsRunSystem
{
    private readonly IMazeBuilder gameContext;

    public PositionUpdateSystem(IMazeBuilder gameContext)
    {
        this.gameContext = gameContext;
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<PositionComponent>()
            .End();

        var positions = world.GetPool<PositionComponent>();

        foreach (var entity in filter)
        {
            var position = positions.Get(entity);
            this.gameContext.UpdatePosition(entity, position);
        }
    }
}
