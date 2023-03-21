using Godot;
using LocomotorECS;

public class MouseInputToDistributionSystem : MatcherEntitySystem
{
    private readonly Map parent;
    private readonly MouseInputDistributionComponent lastMouse = new MouseInputDistributionComponent();

    public MouseInputToDistributionSystem(Map parent) : base(new Matcher().All<MouseInputDistributionComponent>())
    {
        this.parent = parent;
        this.parent.UnhandledInput += UnhandledInput;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        var currentMouse = entity.GetComponent<MouseInputDistributionComponent>();
        var buttonsChanged = currentMouse.MouseButtons ^ lastMouse.MouseButtons;

        currentMouse.MousePosition = lastMouse.MousePosition;
        currentMouse.MouseButtons = lastMouse.MouseButtons;
        currentMouse.JustPressedButtins = buttonsChanged & lastMouse.MouseButtons;
        currentMouse.JustReleasedButtins = buttonsChanged & (~lastMouse.MouseButtons);
    }

    public void UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouse mouse)
        {
            var pos = parent.GetLocalMousePosition();
            this.lastMouse.MousePosition = new System.Numerics.Vector2(pos.x, pos.y);
            this.lastMouse.MouseButtons = mouse.ButtonMask;
        }
    }
}
