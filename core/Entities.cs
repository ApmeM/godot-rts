using System;
using System.Collections.Generic;
using System.Numerics;
using Leopotam.EcsLite;

public class Entities
{
    public static int Build(EcsWorld world, EntityTypeComponent.EntityTypes type, int playerId)
    {
        var entity = world.NewEntity();

        var players = world.GetPool<PlayerComponent>();
        var entityTypes = world.GetPool<EntityTypeComponent>();

        players.GetAdd(entity).PlayerId = playerId;
        entityTypes.GetAdd(entity).EntityType = type;

        switch (type)
        {
            case EntityTypeComponent.EntityTypes.ArtificialWell: BuildArificialWell(world, entity);break;
            case EntityTypeComponent.EntityTypes.House: BuildHouse(world, entity);break;
            case EntityTypeComponent.EntityTypes.Person: BuildPerson(world, entity);break;
            case EntityTypeComponent.EntityTypes.Tree: BuildTree(world, entity);break;
            case EntityTypeComponent.EntityTypes.Well: BuildWell(world, entity);break;
            default: throw new Exception("");
        }

        return entity;
    }

    private static void BuildPerson(EcsWorld world, int entity)
    {
        world.GetPool<SelectableComponent>().GetAdd(entity);
        world.GetPool<PersonComponent>().GetAdd(entity);
        world.GetPool<PositionComponent>().GetAdd(entity);
        world.GetPool<MovingComponent>().GetAdd(entity).MoveSpeed = 64;
        world.GetPool<MovingComponent>().GetAdd(entity).PathTarget = Vector2Ext.Inf;
        world.GetPool<MovingComponent>().GetAdd(entity).Path = new List<Vector2>();
        world.GetPool<DrinkThristingComponent>().GetAdd(entity).CurrentThristing = 100;
        world.GetPool<DrinkThristingComponent>().GetAdd(entity).ThristThreshold = 50;
        world.GetPool<DrinkThristingComponent>().GetAdd(entity).DoneThreshold = 99;
        world.GetPool<DrinkThristingComponent>().GetAdd(entity).MaxThristLevel = 100;
        world.GetPool<DrinkThristingComponent>().GetAdd(entity).ThristSpeed = 3;
        world.GetPool<DrinkThristingComponent>().GetAdd(entity).DrinkSpeed = 50;
        world.GetPool<PrintComponent>().GetAdd(entity);
        world.GetPool<BuilderComponent>().GetAdd(entity).BuildSpeed = 1;
        world.GetPool<PersonDecisionWalkComponent>().GetAdd(entity);
        world.GetPool<PersonDecisionDrinkComponent>().GetAdd(entity);
        world.GetPool<PersonDecisionSleepComponent>().GetAdd(entity);
        world.GetPool<PersonDecisionBuildComponent>().GetAdd(entity);
        world.GetPool<FatigueComponent>().GetAdd(entity).MaxFatigue = 100;
        world.GetPool<FatigueComponent>().GetAdd(entity).FatigueThreshold = 80;
        world.GetPool<FatigueComponent>().GetAdd(entity).FatigueSpeed = 1f;
        world.GetPool<FatigueComponent>().GetAdd(entity).FatigueBuilderSpeed = 5f;
        world.GetPool<FatigueComponent>().GetAdd(entity).DefaultRest = 5f;
    }

    private static void BuildHouse(EcsWorld world, int entity)
    {
        world.GetPool<MouseInputComponent>().GetAdd(entity);
        world.GetPool<SelectableComponent>().GetAdd(entity);
        world.GetPool<PositionComponent>().GetAdd(entity).BlockingCells = new Vector2[]{
            Vector2Ext.Up,
            Vector2Ext.Down,
            Vector2Ext.Left,
            Vector2Ext.Left + Vector2Ext.Up,
            Vector2Ext.Left + Vector2Ext.Down,
            Vector2Ext.Right + Vector2Ext.Up,
            Vector2Ext.Right + Vector2Ext.Down,
        };
        world.GetPool<HPComponent>().GetAdd(entity).MaxHP = 100;
        world.GetPool<AvailabilityComponent>().GetAdd(entity).MaxNumberOfUsers = 10;
        world.GetPool<ConstructionComponent>().GetAdd(entity).MaxBuildProgress = 20;
        world.GetPool<ConstructionComponent>().GetAdd(entity).ConstructionDone = (e) =>
        {
            world.GetPool<AvailabilityComponent>().GetAdd(entity).MaxNumberOfUsers = 10;
            world.GetPool<RestComponent>().GetAdd(entity).Regeneration = 20;
        };
    }

    private static void BuildArificialWell(EcsWorld world, int entity)
    {
        world.GetPool<MouseInputComponent>().GetAdd(entity);
        world.GetPool<SelectableComponent>().GetAdd(entity);
        world.GetPool<PositionComponent>().GetAdd(entity).BlockingCells = new Vector2[]{
            Vector2Ext.Up,
            Vector2Ext.Down,
            Vector2Ext.Left,
            Vector2Ext.Left + Vector2Ext.Up,
            Vector2Ext.Left + Vector2Ext.Down,
            Vector2Ext.Right + Vector2Ext.Up,
            Vector2Ext.Right + Vector2Ext.Down,
        };
        world.GetPool<HPComponent>().GetAdd(entity).MaxHP = 50;
        world.GetPool<AvailabilityComponent>().GetAdd(entity).MaxNumberOfUsers = 10;
        world.GetPool<ConstructionComponent>().GetAdd(entity).MaxBuildProgress = 20;
        world.GetPool<ConstructionComponent>().GetAdd(entity).ConstructionDone = (e) =>
        {
            world.GetPool<AvailabilityComponent>().GetAdd(entity).MaxNumberOfUsers = 5;
            world.GetPool<DrinkableComponent>().GetAdd(entity).CurrentAmount = 0;
            world.GetPool<DrinkableRegenerationComponent>().GetAdd(entity).Regeneration = 25;
            world.GetPool<DrinkableRegenerationComponent>().GetAdd(entity).MaxAmount = 1000;
        };
    }

    private static void BuildTree(EcsWorld world, int entity)
    {
        world.GetPool<PositionComponent>().GetAdd(entity).BlockingCells = new Vector2[] { Vector2.Zero };
    }

    private static void BuildWell(EcsWorld world, int entity)
    {
        world.GetPool<MouseInputComponent>().GetAdd(entity);
        world.GetPool<SelectableComponent>().GetAdd(entity);
        world.GetPool<PositionComponent>().GetAdd(entity);
        world.GetPool<AvailabilityComponent>().GetAdd(entity).MaxNumberOfUsers = 1;
        world.GetPool<DrinkableComponent>().GetAdd(entity).CurrentAmount = 50;
        world.GetPool<DrinkableRegenerationComponent>().GetAdd(entity).MaxAmount = 100;
        world.GetPool<DrinkableRegenerationComponent>().GetAdd(entity).Regeneration = 10;
    }
}
