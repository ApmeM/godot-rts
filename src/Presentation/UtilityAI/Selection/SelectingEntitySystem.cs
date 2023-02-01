using Godot;
using LocomotorECS;

public class SelectingEntitySystem : MatcherEntitySystem
{
    public SelectingEntitySystem() : base(new Matcher()
            .All<SelectableComponent>()
            .All<MouseInputComponent>()
            .All<PositionComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        var mouse = entity.GetComponent<MouseInputComponent>();
        var position = entity.GetComponent<PositionComponent>();

        if ((mouse.MouseButtons & (int)Godot.ButtonList.MaskLeft) == (int)Godot.ButtonList.Left)
        {
            if ((mouse.MousePosition - position.Position).LengthSquared() < 256)
            {
                entity.GetOrCreateComponent<SelectedComponent>().Enable();
            }
            else
            {
                entity.GetOrCreateComponent<SelectedComponent>().Disable();
            }
        }
    }
}