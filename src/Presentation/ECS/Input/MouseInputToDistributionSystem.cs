using Godot;
using Leopotam.EcsLite;

public class MouseInputToDistributionSystem : IEcsRunSystem
{
    private readonly Map parent;

    public System.Numerics.Vector2 MousePosition;
    public int MouseButtons;


    public MouseInputToDistributionSystem(Map parent)
    {
        this.parent = parent;
        this.parent.UnhandledInput += UnhandledInput;
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<MouseInputDistributionComponent>()
            .End();

        var distributions = world.GetPool<MouseInputDistributionComponent>();

        foreach (var entity in filter)
        {
            ref var currentMouse = ref distributions.GetAdd(entity);
            var buttonsChanged = currentMouse.MouseButtons ^ this.MouseButtons;

            currentMouse.MousePosition = this.MousePosition;
            currentMouse.MouseButtons = this.MouseButtons;
            currentMouse.JustPressedButtins = buttonsChanged & this.MouseButtons;
            currentMouse.JustReleasedButtins = buttonsChanged & (~this.MouseButtons);
        }
    }

    public void UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouse mouse)
        {
            var pos = parent.GetLocalMousePosition();
            this.MousePosition = new System.Numerics.Vector2(pos.x, pos.y);
            this.MouseButtons = mouse.ButtonMask;
        }
    }
}
