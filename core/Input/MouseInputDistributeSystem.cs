using LocomotorECS;

public class MouseInputDistributeSystem : MatcherEntitySystem
{
    private readonly Entity inputEntity;

    public MouseInputDistributeSystem(Entity inputEntity) : base(new Matcher().All<MouseInputComponent>())
    {
        this.inputEntity = inputEntity;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        var currentMouse = inputEntity.GetComponent<MouseInputDistributionComponent>();
        var newMouse = entity.GetComponent<MouseInputComponent>();

        newMouse.MousePosition = currentMouse.MousePosition;
        newMouse.MouseButtons = currentMouse.MouseButtons;
        newMouse.JustPressedButtins = currentMouse.JustPressedButtins;
        newMouse.JustReleasedButtins = currentMouse.JustReleasedButtins;
    }
}
