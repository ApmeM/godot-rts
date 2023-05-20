namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class BuildMoveUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<PositionComponent> positions;
        private EcsPool<ConstructionComponent> constructions;
        private EcsPool<PersonDecisionBuildComponent> decisionBuilds;
        private EcsPool<BuilderComponent> builders;
        private EcsPool<MovingComponent> movings;
        private EcsPool<PlayerComponent> players;
        private EcsPool<AvailabilityComponent> availabilities;
        private EcsPool<AvailabilityHolderComponent> holders;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new BuildMoveUpdateSystem());
            systems.Init();

            positions = world.GetPool<PositionComponent>();
            constructions = world.GetPool<ConstructionComponent>();
            decisionBuilds = world.GetPool<PersonDecisionBuildComponent>();
            builders = world.GetPool<BuilderComponent>();
            movings = world.GetPool<MovingComponent>();
            players = world.GetPool<PlayerComponent>();
            availabilities = world.GetPool<AvailabilityComponent>();
            holders = world.GetPool<AvailabilityHolderComponent>();
        }

        [Test]
        public void ConstructionExists_MoveToIt()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionBuilds.Add(testEntity);
            builders.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction);
            players.Add(construction);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void ConstructionNotExists_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionBuilds.Add(testEntity);
            builders.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void ConstructionForAnotherPlayer_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionBuilds.Add(testEntity);
            builders.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity).PlayerId = 1;

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction);
            players.Add(construction);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void ConstructionNotAvailable_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionBuilds.Add(testEntity);
            builders.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction);
            availabilities.Add(construction);
            players.Add(construction);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void ConstructionAvailable_MoveToIt()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionBuilds.Add(testEntity);
            builders.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction);
            availabilities.Add(construction).MaxNumberOfUsers = 1;
            players.Add(construction);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void ConstructionOccupied_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionBuilds.Add(testEntity);
            builders.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction);
            availabilities.Add(construction).MaxNumberOfUsers = 1;
            players.Add(construction);

            var holderEntity = world.NewEntity();
            positions.Add(holderEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionBuilds.Add(holderEntity);
            holders.Add(holderEntity).OccupiedEntity = construction;
            builders.Add(holderEntity);
            movings.Add(holderEntity);
            players.Add(holderEntity);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void MultipleConstructionsAvailable_MoveToClosest()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionBuilds.Add(testEntity);
            builders.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var construction1 = world.NewEntity();
            positions.Add(construction1).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction1);
            players.Add(construction1);
            var construction2 = world.NewEntity();
            positions.Add(construction2).Position = new System.Numerics.Vector2(5, 5);
            constructions.Add(construction2);
            players.Add(construction2);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(5, 5), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void MultipleConstructionsClosestNotAvailable_MoveToAnother()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionBuilds.Add(testEntity);
            builders.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var construction1 = world.NewEntity();
            positions.Add(construction1).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction1);
            players.Add(construction1);
            var construction2 = world.NewEntity();
            positions.Add(construction2).Position = new System.Numerics.Vector2(5, 5);
            constructions.Add(construction2);
            availabilities.Add(construction2);
            players.Add(construction2);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void DoNotWantToBuild_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            builders.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction);
            players.Add(construction);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void NotABuilder_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionBuilds.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            constructions.Add(construction);
            players.Add(construction);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void ConstructionAppearedInProcess_MoveToIt()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionBuilds.Add(testEntity);
            builders.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var construction = world.NewEntity();
            positions.Add(construction).Position = new System.Numerics.Vector2(10, 10);
            players.Add(construction);

            systems.Run();
            constructions.Add(construction);
            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }
    }
}
