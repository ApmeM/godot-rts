using Godot;
using LocomotorECS;

public class MouseInputSystem : MatcherEntitySystem
{
    private readonly Node2D parent;

    public MouseInputSystem(Node2D parent) : base(new Matcher().All<MouseInputComponent>())
    {
        this.parent = parent;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        entity.GetComponent<MouseInputComponent>().MousePosition = this.parent.GetLocalMousePosition();
        entity.GetComponent<MouseInputComponent>().MouseButtons = Input.GetMouseButtonMask();
    }
}
