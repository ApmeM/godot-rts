using System;
using Godot;
using LocomotorECS;

public class Entities
{

    public static Entity BuildPerson()
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
    
    public static Entity BuildHouse()
    {
        var entity = new Entity();
        entity.GetOrCreateComponent<MouseInputComponent>();
        entity.GetOrCreateComponent<SelectableComponent>();
        entity.GetOrCreateComponent<EntityTypeComponent>().EntityType = EntityTypeComponent.EntityTypes.House;
        entity.GetOrCreateComponent<PositionComponent>().BlockingCells = new Vector2[]{
            Vector2.Up,
            Vector2.Down,
            Vector2.Left,
            Vector2.Left + Vector2.Up,
            Vector2.Left + Vector2.Down,
            Vector2.Right + Vector2.Up,
            Vector2.Right + Vector2.Down,
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

    public static Entity BuildArificialWell()
    {
        var entity = new Entity();
        entity.GetOrCreateComponent<MouseInputComponent>();
        entity.GetOrCreateComponent<SelectableComponent>();
        entity.GetOrCreateComponent<EntityTypeComponent>().EntityType = EntityTypeComponent.EntityTypes.ArtificialWell;
        entity.GetOrCreateComponent<PositionComponent>().BlockingCells = new Vector2[]{
            Vector2.Up,
            Vector2.Down,
            Vector2.Left,
            Vector2.Left + Vector2.Up,
            Vector2.Left + Vector2.Down,
            Vector2.Right + Vector2.Up,
            Vector2.Right + Vector2.Down,
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

    public static Entity BuildTree()
    {
        var entity = new Entity();
        entity.GetOrCreateComponent<EntityTypeComponent>().EntityType = EntityTypeComponent.EntityTypes.Tree;
        entity.GetOrCreateComponent<PositionComponent>().BlockingCells = new Vector2[] { Vector2.Zero };
        return entity;
    }

    public static Entity BuildWell()
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
