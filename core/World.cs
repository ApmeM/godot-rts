using System;
using System.Numerics;
using Leopotam.EcsLite;

public class World
{
    public World(Func<Vector2, Vector2> worldToMap, Func<Vector2, Vector2> mapToWorld, float stepX, float stepY)
    {
        this.context = new GameContext(worldToMap, mapToWorld);
        this.world = new EcsWorld();

        this.world.GetPool<MouseInputDistributionComponent>().Add(this.world.NewEntity());

        this.world.GetPool<NotificationComponent>().Add(this.world.NewEntity());

        this.input_esl = new EcsSystems(this.world, sharedData);
        this.input_esl.Add(new MouseInputDistributeSystem());

        this.esl = new EcsSystems(this.world, sharedData);
        this.esl.Add(new RemoveDeadUpdateSystem());

        this.esl.Add(new BuildMoveUpdateSystem());
        this.esl.Add(new BuildProcessUpdateSystem());
        this.esl.Add(new BuildFinishUpdateSystem());
        this.esl.Add(new NotificationsCleanupUpdateSystem());
        this.esl.Add(new DrinkableRegenerationUpdateSystem());
        this.esl.Add(new DrinkMoveUpdateSystem());
        this.esl.Add(new DrinkProcessUpdateSystem());
        this.esl.Add(new DrinkThristingDeathUpdateSystem());
        this.esl.Add(new DrinkThristingUpdateSystem());
        this.esl.Add(new FatigueProcessUpdateSystem());
        this.esl.Add(new FatigueMoveUpdateSystem());
        this.esl.Add(new FatigueSleepingUpdateSystem());
        this.esl.Add(new FatigueToSleepUpdateSystem());
        this.esl.Add(new PersonDecisioWhileSleepUpdateSystem());
        this.esl.Add(new FollowMouseUpdateSystem());
        this.esl.Add(new MovingUpdateSystem(this.context));
        this.esl.Add(new PersonDecisionUpdateSystem());
        this.esl.Add(new PositionBindToMapUpdateSystem(this.context));
        this.esl.Add(new PositionUpdateSystem(this.context));
        this.esl.Add(new ReproductionUpdateSystem());
        this.esl.Add(new SelectingEntitySystem((stepX + stepY) / 2));
        this.esl.Add(new SelectPositionMouseSystem());
        this.esl.Add(new WalkingUpdateSystem());

        // this.esl.AddExecutionOrder<CleanupNotificationsUpdateSystem, DrinkThristingDeathUpdateSystem>();
        // this.esl.AddExecutionOrder<CleanupNotificationsUpdateSystem, FatigueToSleepUpdateSystem>();
        // this.esl.AddExecutionOrder<DrinkThristingDeathUpdateSystem, RemoveDeadSystem>();
        // this.esl.AddExecutionOrder<FollowMouseSystem, PositionBindToMapSystem>();
        // this.esl.AddExecutionOrder<PositionBindToMapSystem, PositionUpdateSystem>();

        this.render_esl = new EcsSystems(this.world, sharedData);
    }

    public void Init()
    {
        this.input_esl.Init();
        this.esl.Init();
        this.render_esl.Init();
    }

    public GameContext context;
    public readonly EcsWorld world;
    public readonly EcsSystems esl;
    public readonly EcsSystems render_esl;
    public readonly EcsSystems input_esl;
    public readonly SharedData sharedData = new SharedData();
    public class SharedData
    {
        public float delta;
    }

    public void Process(float delta)
    {
        sharedData.delta = delta;
        input_esl.Run();
        esl.Run();
        render_esl.Run();
    }

    public void BuildForTest(float stepX, float stepY)
    {
        const int myPlayerId = 1;
        const int fenceSize = 60;
        const int personsCountRows = 6;
        const int personsCountCols = 6;
        const int numberOfWells = 4;
        const int numberOfHouses = 4;

        world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.Well, myPlayerId)).Position = new Vector2(2 * stepX, 2 * stepY);

        for (var i = 0; i < numberOfHouses; i++)
        {
            world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.House, myPlayerId)).Position = new Vector2(5 * stepX, (2 + 5 * i) * stepY);
        }

        for (var i = 0; i < numberOfWells; i++)
        {
            world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.ArtificialWell, myPlayerId)).Position = new Vector2(10 * stepX, (2 + 5 * i) * stepY);
        }

        for (var i = 0; i < personsCountRows; i++)
            for (var j = 0; j < personsCountCols; j++)
            {
                world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.Person, myPlayerId)).Position = new Vector2((15 + i) * stepX, (15 + j) * stepY);
            }

        world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.Tree, 0)).Position = new Vector2(0, 0);
        world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.Tree, 0)).Position = new Vector2(fenceSize * stepX, 0);
        world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.Tree, 0)).Position = new Vector2(0, fenceSize * stepY);
        world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.Tree, 0)).Position = new Vector2(fenceSize * stepX, fenceSize * stepY);

        for (var i = 1; i < fenceSize; i++)
        {
            world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.Tree, 0)).Position = new Vector2(0, i * stepY);
            world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.Tree, 0)).Position = new Vector2(i * stepX, 0);
            world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.Tree, 0)).Position = new Vector2(fenceSize * stepX, i * stepY);
            world.GetPool<PositionComponent>().GetAdd(Entities.Build(world, EntityTypeComponent.EntityTypes.Tree, 0)).Position = new Vector2(i * stepX, fenceSize * stepY);
        }
    }
}
