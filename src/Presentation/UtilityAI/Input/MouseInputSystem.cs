using Godot;
using LocomotorECS;

public class MouseInputSystem : MatcherEntitySystem
{
    private readonly Map parent;
    private MouseInputComponent lastMouse = new MouseInputComponent();

    public MouseInputSystem(Map parent) : base(new Matcher().All<MouseInputComponent>())
    {
        this.parent = parent;
        this.parent.UnhandledInput += UnhandledInput;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        entity.GetComponent<MouseInputComponent>().MousePosition = lastMouse.MousePosition;
        entity.GetComponent<MouseInputComponent>().MouseButtons = lastMouse.MouseButtons;
    }

    public void UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouse mouse)
        {
            this.lastMouse.MousePosition = parent.GetLocalMousePosition();
            this.lastMouse.MouseButtons = mouse.ButtonMask;
        }
    }
}
