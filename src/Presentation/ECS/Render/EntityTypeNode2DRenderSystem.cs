using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Leopotam.EcsLite;

public class EntityTypeNode2DRenderSystem : IEcsRunSystem
{
    public interface IEntityNode2D
    {
        int e { set; }
        EcsWorld world {set;}
        EntityTypeComponent.EntityTypes EntityType { get; }
        int PlayerId { get; }
    }

    private Dictionary<EntityTypeComponent.EntityTypes, PackedScene> sources;
    private readonly Node2D parent;

    public EntityTypeNode2DRenderSystem(Node2D parent)
    {
        this.sources = Enum.GetValues(typeof(EntityTypeComponent.EntityTypes))
                            .Cast<EntityTypeComponent.EntityTypes>()
                            .ToDictionary(a => a, a => ResourceLoader.Load<PackedScene>($"res://Presentation/{a}.tscn"));
        this.parent = parent;
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Exc<Node2DComponent>()
            .Inc<EntityTypeComponent>()
            .End();

        var nodes = world.GetPool<Node2DComponent>();
        var types = world.GetPool<EntityTypeComponent>();

        foreach (var entity in filter)
        {
            var entityType = types.Get(entity).EntityType;
            var scene = this.sources[entityType];
            var inst = scene.Instance<Node2D>();
            this.parent.AddChild(inst);
            nodes.Add(entity).Node = inst;
            ((IEntityNode2D)inst).e = entity;
            ((IEntityNode2D)inst).world = world;
        }
    }
}
