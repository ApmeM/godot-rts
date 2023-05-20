using Godot;
using Leopotam.EcsLite;

public static class WorldExt
{
    public static void BuildFromDesignTime(this EcsWorld world, Map map)
    {
        foreach (Node2D child in map.GetChildren())
        {
            if (!(child is EntityTypeNode2DRenderSystem.IEntityNode2D etn))
            {
                return;
            }

            world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, etn.EntityType, etn.PlayerId)).Position = new System.Numerics.Vector2(child.Position.x, child.Position.y);
        }
    }

    public static void AddGodotSpecific(this World world, Map map)
    {
        world.input_esl.Add(new MouseInputToDistributionSystem(map));
        // world.input_esl.AddExecutionOrder<MouseInputToDistributionSystem, MouseInputDistributeSystem>();

        world.render_esl.Add(new EntityTypeNode2DRenderSystem(map));
        world.render_esl.Add(new Node2DPositionRenderSystem());
        world.render_esl.Add(new Node2DDyingRenderSystem());

    }
}
