namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class BuildProgressUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<PositionComponent> positions;
        private EcsPool<ConstructionComponent> constructions;
        private EcsPool<PersonDecisionBuildComponent> decisionBuilds;
        private EcsPool<BuilderComponent> builders;
        private EcsPool<PlayerComponent> players;
        private EcsPool<AvailabilityComponent> availabilities;
        private EcsPool<AvailabilityHolderComponent> holders;
        private EcsPool<HPComponent> hps;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new BuildProcessUpdateSystem());
            systems.Init();

            positions = world.GetPool<PositionComponent>();
            constructions = world.GetPool<ConstructionComponent>();
            decisionBuilds = world.GetPool<PersonDecisionBuildComponent>();
            builders = world.GetPool<BuilderComponent>();
            players = world.GetPool<PlayerComponent>();
            availabilities = world.GetPool<AvailabilityComponent>();
            holders = world.GetPool<AvailabilityHolderComponent>();
            hps = world.GetPool<HPComponent>();
        }

        [Test]
        public void AtConstruction_Build()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionBuilds.Add(testEntity);
            players.Add(testEntity);
            builders.Add(testEntity).BuildSpeed = 1;

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction).MaxBuildProgress = 10;
            players.Add(construction);

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(0.1f, constructions.Get(construction).BuildProgress);
        }

        [Test]
        public void ConstructionHasHP_AlsoIncreased()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionBuilds.Add(testEntity);
            players.Add(testEntity);
            builders.Add(testEntity).BuildSpeed = 1;

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction).MaxBuildProgress = 5;
            players.Add(construction);
            hps.Add(construction).MaxHP = 10;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(0.2f, hps.Get(construction).HP);
        }

        [Test]
        public void ConstructionAlmostDone_ProgressOnlyTillTheEnd()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionBuilds.Add(testEntity);
            players.Add(testEntity);
            builders.Add(testEntity).BuildSpeed = 1;

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction).MaxBuildProgress = 5;
            constructions.Get(construction).BuildProgress = 4.99f;
            players.Add(construction);
            hps.Add(construction).MaxHP = 10;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(0.02f, hps.Get(construction).HP, 0.001);
            Assert.AreEqual(5, constructions.Get(construction).BuildProgress);
        }

        [Test]
        public void EntityStartBuilding_BuildingOccupied()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionBuilds.Add(testEntity);
            players.Add(testEntity);
            builders.Add(testEntity).BuildSpeed = 1;

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction).MaxBuildProgress = 5;
            players.Add(construction);

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(construction, holders.Get(testEntity).OccupiedEntity);
        }

        [Test]
        public void TwoBuildersTryingToBuild_OnlyFirstBuilds()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionBuilds.Add(testEntity);
            players.Add(testEntity);
            builders.Add(testEntity).BuildSpeed = 1;

            var testEntity2 = world.NewEntity();
            positions.Add(testEntity2).Position = new System.Numerics.Vector2(10, 10);
            decisionBuilds.Add(testEntity2);
            players.Add(testEntity2);
            builders.Add(testEntity2).BuildSpeed = 1;

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction).MaxBuildProgress = 5;
            players.Add(construction);
            availabilities.Add(construction).MaxNumberOfUsers = 1;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(construction, holders.Get(testEntity).OccupiedEntity);
            Assert.AreEqual(false, holders.Has(testEntity2));
            Assert.AreEqual(0.1f, constructions.Get(construction).BuildProgress);
        }


        [Test]
        public void ConstructionAppearedInProcess_Build()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionBuilds.Add(testEntity);
            players.Add(testEntity);
            builders.Add(testEntity).BuildSpeed = 1;

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            players.Add(construction);

            sharedData.delta = 0.1f;
            systems.Run();
            constructions.Add(construction).MaxBuildProgress = 10;
            systems.Run();

            Assert.AreEqual(0.1f, constructions.Get(construction).BuildProgress);
        }
    }
}
