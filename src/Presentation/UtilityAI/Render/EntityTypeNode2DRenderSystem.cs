using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using LocomotorECS;

public class EntityTypeNode2DRenderSystem : MatcherEntitySystem
{
    public interface IEntityNode2D
    {
        Entity e { set; }
    }

    private Dictionary<EntityTypeComponent.EntityTypes, PackedScene> sources;
    private readonly Node2D parent;

    public EntityTypeNode2DRenderSystem(Node2D parent) : base(new Matcher().Exclude<Node2DComponent>().All<EntityTypeComponent>())
    {
        this.sources = Enum.GetValues(typeof(EntityTypeComponent.EntityTypes))
                            .Cast<EntityTypeComponent.EntityTypes>()
                            .ToDictionary(a => a, a => ResourceLoader.Load<PackedScene>($"res://Presentation/{a}.tscn"));
        this.parent = parent;
    }

    protected override void OnEntityListChanged(HashSet<Entity> added, HashSet<Entity> changed, HashSet<Entity> removed)
    {
        base.OnEntityListChanged(added, changed, removed);

        foreach (var entity in added)
        {
            var entityType = entity.GetComponent<EntityTypeComponent>().EntityType;
            var scene = this.sources[entityType];
            var inst = scene.Instance<Node2D>();
            this.parent.AddChild(inst);
            entity.AddComponent<Node2DComponent>().Node = inst;
            (inst as IEntityNode2D).e = entity;
        }
    }

    protected override void DoAction(Entity entity, float delta)
    {
    }
}
