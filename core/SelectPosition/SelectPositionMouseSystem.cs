using Leopotam.EcsLite;

public class SelectPositionMouseSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<SelectPositionMouseComponent>()
            .Inc<MouseInputComponent>()
            .Inc<EntityTypeComponent>()
            .Inc<PositionComponent>()
            .Inc<PlayerComponent>()
            .End();

        var mouseInputs = world.GetPool<MouseInputComponent>();
        var positions = world.GetPool<PositionComponent>();
        var entityTypes = world.GetPool<EntityTypeComponent>();
        var players = world.GetPool<PlayerComponent>();

        foreach (var entity in filter)
        {
            if ((mouseInputs.Get(entity).MouseButtons & (int)MouseInputComponent.ButtonList.MaskLeft) != (int)MouseInputComponent.ButtonList.Left)
            {
                continue;
            }

            var newEntity = Entities.Build(world, entityTypes.GetAdd(entity).EntityType, players.GetAdd(entity).PlayerId);
            positions.GetAdd(newEntity).Position = positions.GetAdd(entity).Position;
            world.DelEntity(entity);
        }
    }
}