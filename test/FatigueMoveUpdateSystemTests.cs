namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class FatigueMoveUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<PositionComponent> positions;
        private EcsPool<RestComponent> rests;
        private EcsPool<PersonDecisionSleepComponent> decisionSleeps;
        private EcsPool<FatigueComponent> fatigues;
        private EcsPool<FatigueSleepComponent> sleeps;
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
            systems.Add(new FatigueMoveUpdateSystem());
            systems.Init();

            positions = world.GetPool<PositionComponent>();
            rests = world.GetPool<RestComponent>();
            decisionSleeps = world.GetPool<PersonDecisionSleepComponent>();
            fatigues = world.GetPool<FatigueComponent>();
            sleeps = world.GetPool<FatigueSleepComponent>();
            movings = world.GetPool<MovingComponent>();
            players = world.GetPool<PlayerComponent>();
            availabilities = world.GetPool<AvailabilityComponent>();
            holders = world.GetPool<AvailabilityHolderComponent>();
        }

        [Test]
        public void RestExists_MoveToIt()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionSleeps.Add(testEntity);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var rest = world.NewEntity();
            positions.Add(rest).Position = new System.Numerics.Vector2(10, 10);
            rests.Add(rest);
            players.Add(rest);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void RestNotExists_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionSleeps.Add(testEntity);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void RestForAnotherPlayer_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionSleeps.Add(testEntity);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity).PlayerId = 1;

            var rest = world.NewEntity();
            positions.Add(rest).Position = new System.Numerics.Vector2(10, 10);
            rests.Add(rest);
            players.Add(rest);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void RestNotAvailable_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionSleeps.Add(testEntity);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var rest = world.NewEntity();
            positions.Add(rest).Position = new System.Numerics.Vector2(10, 10);
            rests.Add(rest);
            availabilities.Add(rest);
            players.Add(rest);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void RestAvailable_MoveToIt()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionSleeps.Add(testEntity);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var rest = world.NewEntity();
            positions.Add(rest).Position = new System.Numerics.Vector2(10, 10);
            rests.Add(rest);
            availabilities.Add(rest).MaxNumberOfUsers = 1;
            players.Add(rest);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void AtRestPosition_FallAsleepInHouse()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionSleeps.Add(testEntity);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var rest = world.NewEntity();
            positions.Add(rest).Position = new System.Numerics.Vector2(10, 10);
            rests.Add(rest);
            availabilities.Add(rest).MaxNumberOfUsers = 1;
            players.Add(rest);

            systems.Run();

            Assert.AreEqual(true, sleeps.Get(testEntity).InHouse);
        }

        [Test]
        public void AtRestPosition_RecoverySpeedTakenFromHouse()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionSleeps.Add(testEntity);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var rest = world.NewEntity();
            positions.Add(rest).Position = new System.Numerics.Vector2(10, 10);
            rests.Add(rest).Regeneration = 12;
            availabilities.Add(rest).MaxNumberOfUsers = 1;
            players.Add(rest);

            systems.Run();

            Assert.AreEqual(12, sleeps.Get(testEntity).RestSpeed);
        }

        [Test]
        public void RestOccupied_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionSleeps.Add(testEntity);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var rest = world.NewEntity();
            positions.Add(rest).Position = new System.Numerics.Vector2(10, 10);
            rests.Add(rest);
            availabilities.Add(rest).MaxNumberOfUsers = 1;
            players.Add(rest);

            var holderEntity = world.NewEntity();
            positions.Add(holderEntity).Position = new System.Numerics.Vector2(10, 10);
            sleeps.Add(holderEntity);
            holders.Add(holderEntity).OccupiedEntity = rest;
            fatigues.Add(holderEntity);
            movings.Add(holderEntity);
            players.Add(holderEntity);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void MultipleRestsAvailable_MoveToClosest()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionSleeps.Add(testEntity);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var rest1 = world.NewEntity();
            positions.Add(rest1).Position = new System.Numerics.Vector2(10, 10);
            rests.Add(rest1);
            players.Add(rest1);
            var rest2 = world.NewEntity();
            positions.Add(rest2).Position = new System.Numerics.Vector2(5, 5);
            rests.Add(rest2);
            players.Add(rest2);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(5, 5), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void MultipleRestsClosestNotAvailable_MoveToAnother()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionSleeps.Add(testEntity);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var rest1 = world.NewEntity();
            positions.Add(rest1).Position = new System.Numerics.Vector2(10, 10);
            rests.Add(rest1);
            players.Add(rest1);
            var rest2 = world.NewEntity();
            positions.Add(rest2).Position = new System.Numerics.Vector2(5, 5);
            rests.Add(rest2);
            availabilities.Add(rest2);
            players.Add(rest2);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void DoNotWantToRest_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var rest = world.NewEntity();
            positions.Add(rest).Position = new System.Numerics.Vector2(10, 10);
            rests.Add(rest);
            players.Add(rest);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void NotAFatigue_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionSleeps.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var rest = world.NewEntity();
            positions.Add(rest).Position = new System.Numerics.Vector2(10, 10);
            rests.Add(rest);
            players.Add(rest);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void RestAppearsInProcess_MoveToIt()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionSleeps.Add(testEntity);
            fatigues.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var rest = world.NewEntity();
            positions.Add(rest).Position = new System.Numerics.Vector2(10, 10);
            players.Add(rest);

            systems.Run();
            rests.Add(rest);
            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }

    }
}
