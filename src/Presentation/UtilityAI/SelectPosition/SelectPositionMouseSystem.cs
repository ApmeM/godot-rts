using Godot;
using LocomotorECS;

public class SelectPositionMouseSystem : MatcherEntitySystem
{
    private readonly World world;

    public SelectPositionMouseSystem(World parent) : base(new Matcher()
            .All<SelectPositionMouseComponent>()
            .All<MouseInputComponent>()
            .All<EntityTypeComponent>()
            .All<PositionComponent>())
    {
        this.world = parent;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        var mouse = entity.GetComponent<MouseInputComponent>();
        if ((mouse.MouseButtons & (int)ButtonList.MaskLeft) != (int)ButtonList.Left)
        {
            return;
        }

        var position = entity.GetComponent<PositionComponent>();
        var type = entity.GetComponent<EntityTypeComponent>();
        var newEntity = Entities.Build(type.EntityType);
        newEntity.GetComponent<PositionComponent>().Position = entity.GetComponent<PositionComponent>().Position;
        this.world.el.Add(newEntity);
        this.world.el.Remove(entity);
    }
}
