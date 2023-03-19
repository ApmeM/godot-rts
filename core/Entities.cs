using System;
using System.Numerics;
using LocomotorECS;

public class Entities
{
    public static Entity Build(EntityTypeComponent.EntityTypes type, int playerId){
        var e = Build(type);
        e.AddComponent<PlayerComponent>().PlayerId = playerId;
        e.Tag = (int)type;
        return e;
    }

    private static Entity Build(EntityTypeComponent.EntityTypes type)
    {
        switch (type)
        {
            case EntityTypeComponent.EntityTypes.ArtificialWell: return BuildArificialWell();
            case EntityTypeComponent.EntityTypes.House: return BuildHouse();
            case EntityTypeComponent.EntityTypes.Person: return BuildPerson();
            case EntityTypeComponent.EntityTypes.Tree: return BuildTree();
            case EntityTypeComponent.EntityTypes.Well: return BuildWell();
            default: throw new Exception("");
        }
    }

    private static Entity BuildPerson()
    {
        var entity = new Entity();
        entity.GetOrCreateComponent<MouseInputComponent>();
        entity.GetOrCreateComponent<SelectableComponent>();
        entity.GetOrCreateComponent<EntityTypeComponent>().EntityType = EntityTypeComponent.EntityTypes.Person;
        entity.GetOrCreateComponent<PersonComponent>();
        entity.GetOrCreateComponent<PositionComponent>();
        entity.GetOrCreateComponent<MovingComponent>().MoveSpeed = 64;
        entity.GetOrCreateComponent<DrinkThristingComponent>();
        entity.GetOrCreateComponent<PrintComponent>();
        entity.GetOrCreateComponent<BuilderComponent>();
        entity.GetOrCreateComponent<PersonDecisionWalkComponent>();
        entity.GetOrCreateComponent<FatigueComponent>().MaxFatigue = 100;
        entity.GetOrCreateComponent<FatigueComponent>().FatigueThreshold = 80;
        entity.GetOrCreateComponent<FatigueComponent>().FatigueSpeed = 1f;
        entity.GetOrCreateComponent<FatigueComponent>().DefaultRest = 5f;
        return entity;
    }

    private static Entity BuildHouse()
    {
        var entity = new Entity();
        entity.GetOrCreateComponent<MouseInputComponent>();
        entity.GetOrCreateComponent<SelectableComponent>();
        entity.GetOrCreateComponent<EntityTypeComponent>().EntityType = EntityTypeComponent.EntityTypes.House;
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

    private static Entity BuildArificialWell()
    {
        var entity = new Entity();
        entity.GetOrCreateComponent<MouseInputComponent>();
        entity.GetOrCreateComponent<SelectableComponent>();
        entity.GetOrCreateComponent<EntityTypeComponent>().EntityType = EntityTypeComponent.EntityTypes.ArtificialWell;
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

    private static Entity BuildTree()
    {
        var entity = new Entity();
        entity.GetOrCreateComponent<EntityTypeComponent>().EntityType = EntityTypeComponent.EntityTypes.Tree;
        entity.GetOrCreateComponent<PositionComponent>().BlockingCells = new Vector2[] { Vector2.Zero };
        return entity;
    }

    private static Entity BuildWell()
    {
        var entity = new Entity();
        entity.GetOrCreateComponent<MouseInputComponent>();
        entity.GetOrCreateComponent<SelectableComponent>();
        entity.GetOrCreateComponent<PositionComponent>();
        entity.GetOrCreateComponent<EntityTypeComponent>().EntityType = EntityTypeComponent.EntityTypes.Well;
        entity.GetOrCreateComponent<AvailabilityComponent>().MaxNumberOfUsers = 1;
        entity.GetOrCreateComponent<DrinkableComponent>().CurrentAmount = 50;
        entity.GetOrCreateComponent<DrinkableRegenerationComponent>().MaxAmount = 100;
        entity.GetOrCreateComponent<DrinkableRegenerationComponent>().Regeneration = 10;
        return entity;
    }
}
