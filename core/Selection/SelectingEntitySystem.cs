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

        if ((mouse.JustPressedButtins & (int)MouseInputComponent.ButtonList.MaskLeft) == (int)MouseInputComponent.ButtonList.Left)
        {
            if ((mouse.MousePosition - position.Position).LengthSquared() < 256)
            {
                entity.GetComponent<SelectedComponent>().Enable();
            }
            else
            {
                entity.GetComponent<SelectedComponent>().Disable();
            }
        }
    }
}