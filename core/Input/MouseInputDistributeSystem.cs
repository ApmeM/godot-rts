using HonkPerf.NET.RefLinq;
using LocomotorECS;

public class MouseInputDistributeSystem : MatcherEntitySystem
{
    private MatcherEntityList inputEntityList;

    public MouseInputDistributeSystem() : base(new Matcher().All<MouseInputComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        var inputEntity = this.inputEntityList.Entities.ToRefLinq().First();

        var currentMouse = inputEntity.GetComponent<MouseInputDistributionComponent>();
        var newMouse = entity.GetComponent<MouseInputComponent>();

        newMouse.MousePosition = currentMouse.MousePosition;
        newMouse.MouseButtons = currentMouse.MouseButtons;
        newMouse.JustPressedButtins = currentMouse.JustPressedButtins;
        newMouse.JustReleasedButtins = currentMouse.JustReleasedButtins;
    }

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.inputEntityList = new MatcherEntityList(entityList, new Matcher().All<MouseInputDistributionComponent>());
        return base.FilterEntityList(entityList);
    }
}
