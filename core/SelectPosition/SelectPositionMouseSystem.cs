using LocomotorECS;

public class SelectPositionMouseSystem : MatcherEntitySystem
{
    private readonly World world;

    public SelectPositionMouseSystem(World parent) : base(new Matcher()
            .All<SelectPositionMouseComponent>()
            .All<MouseInputComponent>()
            .All<EntityTypeComponent>()
            .All<PositionComponent>()
            .All<PlayerComponent>())
    {
        this.world = parent;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        var mouse = entity.GetComponent<MouseInputComponent>();
        if ((mouse.MouseButtons & (int)MouseInputComponent.ButtonList.MaskLeft) != (int)MouseInputComponent.ButtonList.Left)
        {
            return;
        }

        var position = entity.GetComponent<PositionComponent>();
        var type = entity.GetComponent<EntityTypeComponent>();
        var player = entity.GetComponent<PlayerComponent>();
        var newEntity = Entities.Build(type.EntityType, player.PlayerId);
        newEntity.GetComponent<PositionComponent>().Position = position.Position;
        this.world.el.Add(newEntity);
        this.world.el.Remove(entity);
    }
}
