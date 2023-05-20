using Leopotam.EcsLite;

public class SelectingEntitySystem : IEcsRunSystem
{
    private readonly float distance;

    public SelectingEntitySystem(float distance)
    {
        this.distance = distance;
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<SelectableComponent>()
            .Inc<MouseInputComponent>()
            .Inc<PositionComponent>()
            .End();

        var selectables = world.GetPool<SelectableComponent>();
        var mouseInputs = world.GetPool<MouseInputComponent>();
        var positions = world.GetPool<PositionComponent>();
        var selecteds = world.GetPool<SelectedComponent>();

        foreach (var entity in filter)
        {
            var mouse = mouseInputs.Get(entity);
            var position = positions.Get(entity);

            if ((mouse.JustPressedButtins & (int)MouseInputComponent.ButtonList.MaskLeft) == (int)MouseInputComponent.ButtonList.Left)
            {
                if ((mouse.MousePosition - position.Position).LengthSquared() < this.distance * this.distance / 2)
                {
                    selecteds.GetAdd(entity);
                }
                else
                {
                    selecteds.Del(entity);
                }
            }
        }
    }
}