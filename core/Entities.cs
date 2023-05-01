using System;
using System.Collections.Generic;
using System.Numerics;
using LocomotorECS;

public class Entities
{
    private static Dictionary<EntityTypeComponent.EntityTypes, Stack<Entity>> cache =
        new Dictionary<EntityTypeComponent.EntityTypes, Stack<Entity>>();

    public static Entity Build(EntityTypeComponent.EntityTypes type, int playerId)
    {
        var entity = cache.ContainsKey(type) ? cache[type].Count > 0 ? cache[type].Pop() : new Entity() : new Entity();

        entity.Tag = (int)type;

        entity.GetOrCreateComponent<PlayerComponent>().PlayerId = playerId;
        entity.GetOrCreateComponent<EntityTypeComponent>().EntityType = type;

        switch (type)
        {
            case EntityTypeComponent.EntityTypes.ArtificialWell: return BuildArificialWell(entity);
            case EntityTypeComponent.EntityTypes.House: return BuildHouse(entity);
            case EntityTypeComponent.EntityTypes.Person: return BuildPerson(entity);
            case EntityTypeComponent.EntityTypes.Tree: return BuildTree(entity);
            case EntityTypeComponent.EntityTypes.Well: return BuildWell(entity);
            default: throw new Exception("");
        }
    }

    private static Entity BuildPerson(Entity entity)
    {
        entity.GetOrCreateComponent<MouseInputComponent>();
        entity.GetOrCreateComponent<SelectableComponent>();
        entity.GetOrCreateComponent<SelectedComponent>().Disable();
        entity.GetOrCreateComponent<PersonComponent>();
        entity.GetOrCreateComponent<PositionComponent>();
        entity.GetOrCreateComponent<MovingComponent>().MoveSpeed = 64;
        entity.GetOrCreateComponent<MovingComponent>().PathTarget = Vector2Ext.Inf;
        entity.GetOrCreateComponent<DrinkThristingComponent>().CurrentThristing = 100;
        entity.GetOrCreateComponent<DrinkThristingComponent>().ThristThreshold = 50;
        entity.GetOrCreateComponent<DrinkThristingComponent>().MaxThristLevel = 100;
        entity.GetOrCreateComponent<DrinkThristingComponent>().ThristSpeed = 3;
        entity.GetOrCreateComponent<DrinkThristingComponent>().DrinkSpeed = 50;
        entity.GetOrCreateComponent<DeadComponent>().Disable();
        entity.GetOrCreateComponent<PrintComponent>();
        entity.GetOrCreateComponent<BuilderComponent>();
        entity.GetOrCreateComponent<PersonDecisionWalkComponent>();
        entity.GetOrCreateComponent<PersonDecisionDrinkComponent>();
        entity.GetOrCreateComponent<PersonDecisionSleepComponent>();
        entity.GetOrCreateComponent<PersonDecisionBuildComponent>();
        entity.GetOrCreateComponent<FatigueSleepComponent>().Disable();
        entity.GetOrCreateComponent<FatigueComponent>().MaxFatigue = 100;
        entity.GetOrCreateComponent<FatigueComponent>().FatigueThreshold = 80;
        entity.GetOrCreateComponent<FatigueComponent>().FatigueSpeed = 1f;
        entity.GetOrCreateComponent<FatigueComponent>().DefaultRest = 5f;
        return entity;
    }

    private static Entity BuildHouse(Entity entity)
    {
        entity.GetOrCreateComponent<MouseInputComponent>();
        entity.GetOrCreateComponent<SelectableComponent>();
        entity.GetOrCreateComponent<SelectedComponent>().Disable();
        entity.GetOrCreateComponent<PositionComponent>().BlockingCells = new Vector2[]{
            Vector2Ext.Up,
            Vector2Ext.Down,
            Vector2Ext.Left,
            Vector2Ext.Left + Vector2Ext.Up,
            Vector2Ext.Left + Vector2Ext.Down,
            Vector2Ext.Right + Vector2Ext.Up,
            Vector2Ext.Right + Vector2Ext.Down,
        };
        entity.GetOrCreateComponent<HPComponent>().MaxHP = 100;
        entity.GetOrCreateComponent<AvailabilityComponent>().MaxNumberOfUsers = 10;
        entity.GetOrCreateComponent<ConstructionComponent>().ConstructionDone = (e) =>
        {
            e.GetOrCreateComponent<AvailabilityComponent>().MaxNumberOfUsers = 10;
            e.GetOrCreateComponent<RestComponent>().Regeneration = 20;
        };
        return entity;
    }

    private static Entity BuildArificialWell(Entity entity)
    {
        entity.GetOrCreateComponent<MouseInputComponent>();
        entity.GetOrCreateComponent<SelectableComponent>();
        entity.GetOrCreateComponent<SelectedComponent>().Disable();
        entity.GetOrCreateComponent<PositionComponent>().BlockingCells = new Vector2[]{
            Vector2Ext.Up,
            Vector2Ext.Down,
            Vector2Ext.Left,
            Vector2Ext.Left + Vector2Ext.Up,
            Vector2Ext.Left + Vector2Ext.Down,
            Vector2Ext.Right + Vector2Ext.Up,
            Vector2Ext.Right + Vector2Ext.Down,
        };
        entity.GetOrCreateComponent<HPComponent>().MaxHP = 50;
        entity.GetOrCreateComponent<AvailabilityComponent>().MaxNumberOfUsers = 10;
        entity.GetOrCreateComponent<ConstructionComponent>().ConstructionDone = (e) =>
        {
            e.GetOrCreateComponent<AvailabilityComponent>().MaxNumberOfUsers = 5;
            e.GetOrCreateComponent<DrinkableComponent>().CurrentAmount = 0;
            e.GetOrCreateComponent<DrinkableRegenerationComponent>().Regeneration = 25;
            e.GetOrCreateComponent<DrinkableRegenerationComponent>().MaxAmount = 1000;
        };
        return entity;
    }

    private static Entity BuildTree(Entity entity)
    {
        entity.GetOrCreateComponent<PositionComponent>().BlockingCells = new Vector2[] { Vector2.Zero };
        return entity;
    }

    private static Entity BuildWell(Entity entity)
    {
        entity.GetOrCreateComponent<MouseInputComponent>();
        entity.GetOrCreateComponent<SelectableComponent>();
        entity.GetOrCreateComponent<SelectedComponent>().Disable();
        entity.GetOrCreateComponent<PositionComponent>();
        entity.GetOrCreateComponent<AvailabilityComponent>().MaxNumberOfUsers = 1;
        entity.GetOrCreateComponent<DrinkableComponent>().CurrentAmount = 50;
        entity.GetOrCreateComponent<DrinkableRegenerationComponent>().MaxAmount = 100;
        entity.GetOrCreateComponent<DrinkableRegenerationComponent>().Regeneration = 10;
        return entity;
    }

    public static void Return(Entity entity)
    {
        var type = entity.GetOrCreateComponent<EntityTypeComponent>().EntityType;
        if (!cache.ContainsKey(type))
        {
            cache[type] = new Stack<Entity>();
        }

        cache[type].Push(entity);
    }
}
