namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class DrinkMoveUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<PositionComponent> positions;
        private EcsPool<DrinkableComponent> drinkables;
        private EcsPool<PersonDecisionDrinkComponent> decisionDrinks;
        private EcsPool<DrinkThristingComponent> thristers;
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
            systems.Add(new DrinkMoveUpdateSystem());
            systems.Init();

            positions = world.GetPool<PositionComponent>();
            drinkables = world.GetPool<DrinkableComponent>();
            decisionDrinks = world.GetPool<PersonDecisionDrinkComponent>();
            thristers = world.GetPool<DrinkThristingComponent>();
            movings = world.GetPool<MovingComponent>();
            players = world.GetPool<PlayerComponent>();
            availabilities = world.GetPool<AvailabilityComponent>();
            holders = world.GetPool<AvailabilityHolderComponent>();
        }

        [Test]
        public void DrinkableExists_MoveToIt()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionDrinks.Add(testEntity);
            thristers.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var drinkable = world.NewEntity();
            positions.Add(drinkable).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(drinkable);
            players.Add(drinkable);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void DrinkableNotExists_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionDrinks.Add(testEntity);
            thristers.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void DrinkableForNeutralPlayer_Move()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionDrinks.Add(testEntity);
            thristers.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity).PlayerId = 1;

            var drinkable = world.NewEntity();
            positions.Add(drinkable).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(drinkable);
            players.Add(drinkable);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void DrinkableForAnotherPlayer_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionDrinks.Add(testEntity);
            thristers.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity).PlayerId = 1;

            var drinkable = world.NewEntity();
            positions.Add(drinkable).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(drinkable);
            players.Add(drinkable).PlayerId = 2;

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void DrinkableNotAvailable_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionDrinks.Add(testEntity);
            thristers.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var drinkable = world.NewEntity();
            positions.Add(drinkable).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(drinkable);
            availabilities.Add(drinkable);
            players.Add(drinkable);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void DrinkableAvailable_MoveToIt()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionDrinks.Add(testEntity);
            thristers.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var drinkable = world.NewEntity();
            positions.Add(drinkable).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(drinkable);
            availabilities.Add(drinkable).MaxNumberOfUsers = 1;
            players.Add(drinkable);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void DrinkableOccupied_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionDrinks.Add(testEntity);
            thristers.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var drinkable = world.NewEntity();
            positions.Add(drinkable).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(drinkable);
            availabilities.Add(drinkable).MaxNumberOfUsers = 1;
            players.Add(drinkable);

            var holderEntity = world.NewEntity();
            positions.Add(holderEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionDrinks.Add(holderEntity);
            holders.Add(holderEntity).OccupiedEntity = drinkable;
            thristers.Add(holderEntity);
            movings.Add(holderEntity);
            players.Add(holderEntity);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void MultipleDrinkablesAvailable_MoveToClosest()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionDrinks.Add(testEntity);
            thristers.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var drinkable1 = world.NewEntity();
            positions.Add(drinkable1).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(drinkable1);
            players.Add(drinkable1);
            var drinkable2 = world.NewEntity();
            positions.Add(drinkable2).Position = new System.Numerics.Vector2(5, 5);
            drinkables.Add(drinkable2);
            players.Add(drinkable2);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(5, 5), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void MultipleDrinkablesClosestNotAvailable_MoveToAnother()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionDrinks.Add(testEntity);
            thristers.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var drinkable1 = world.NewEntity();
            positions.Add(drinkable1).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(drinkable1);
            players.Add(drinkable1);
            var drinkable2 = world.NewEntity();
            positions.Add(drinkable2).Position = new System.Numerics.Vector2(5, 5);
            drinkables.Add(drinkable2);
            availabilities.Add(drinkable2);
            players.Add(drinkable2);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void DoNotWantToBuild_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            thristers.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var drinkable = world.NewEntity();
            positions.Add(drinkable).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(drinkable);
            players.Add(drinkable);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void NotABuilder_DoNotMove()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionDrinks.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var drinkable = world.NewEntity();
            positions.Add(drinkable).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(drinkable);
            players.Add(drinkable);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(0, 0), movings.Get(testEntity).PathTarget);
        }

        [Test]
        public void DrinkableAppearedinProcess_MoveToIt()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(1, 1);
            decisionDrinks.Add(testEntity);
            thristers.Add(testEntity);
            movings.Add(testEntity);
            players.Add(testEntity);

            var drinkable = world.NewEntity();
            positions.Add(drinkable).Position = new System.Numerics.Vector2(10, 10);
            players.Add(drinkable);

            systems.Run();
            drinkables.Add(drinkable);
            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(10, 10), movings.Get(testEntity).PathTarget);
        }    }
}
