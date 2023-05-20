namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class DrinkProgressUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<PositionComponent> positions;
        private EcsPool<DrinkableComponent> drinkables;
        private EcsPool<PersonDecisionDrinkComponent> decisionDrinks;
        private EcsPool<DrinkThristingComponent> thristers;
        private EcsPool<PlayerComponent> players;
        private EcsPool<AvailabilityComponent> availabilities;
        private EcsPool<AvailabilityHolderComponent> holders;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new DrinkProcessUpdateSystem());
            systems.Init();

            positions = world.GetPool<PositionComponent>();
            drinkables = world.GetPool<DrinkableComponent>();
            decisionDrinks = world.GetPool<PersonDecisionDrinkComponent>();
            thristers = world.GetPool<DrinkThristingComponent>();
            players = world.GetPool<PlayerComponent>();
            availabilities = world.GetPool<AvailabilityComponent>();
            holders = world.GetPool<AvailabilityHolderComponent>();
        }

        [Test]
        public void AtWater_Drink()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionDrinks.Add(testEntity);
            players.Add(testEntity);
            thristers.Add(testEntity).DrinkSpeed = 1;
            thristers.Get(testEntity).MaxThristLevel = 1;

            var water = world.NewEntity();
            positions.Add(water).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(water).CurrentAmount = 10;
            players.Add(water);

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(0.1f, thristers.Get(testEntity).CurrentThristing);
            Assert.AreEqual(9.9f, drinkables.Get(water).CurrentAmount);
        }

        [Test]
        public void WaterAlmostEmpty_ProgressOnlyTillTheEnd()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionDrinks.Add(testEntity);
            players.Add(testEntity);
            thristers.Add(testEntity).DrinkSpeed = 1;
            thristers.Get(testEntity).MaxThristLevel = 1;

            var water = world.NewEntity();
            positions.Add(water).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(water).CurrentAmount = 0.0001f;
            players.Add(water);

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(0.0001f, thristers.Get(testEntity).CurrentThristing);
            Assert.AreEqual(0f, drinkables.Get(water).CurrentAmount);
        }

        [Test]
        public void AlmostFull_ProgressOnlyTillTheEnd()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionDrinks.Add(testEntity);
            players.Add(testEntity);
            thristers.Add(testEntity).DrinkSpeed = 1;
            thristers.Get(testEntity).MaxThristLevel = 5;
            thristers.Get(testEntity).CurrentThristing = 4.999f;

            var water = world.NewEntity();
            positions.Add(water).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(water).CurrentAmount = 10;
            players.Add(water);

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(5, thristers.Get(testEntity).CurrentThristing);
            Assert.AreEqual(9.999, drinkables.Get(water).CurrentAmount, 0.000001f);
        }

        [Test]
        public void EntityStartDrinking_BuildingOccupied()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionDrinks.Add(testEntity);
            players.Add(testEntity);
            thristers.Add(testEntity).DrinkSpeed = 1;
            thristers.Get(testEntity).MaxThristLevel = 1;

            var water = world.NewEntity();
            positions.Add(water).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(water).CurrentAmount = 5;
            players.Add(water);

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(water, holders.Get(testEntity).OccupiedEntity);
        }

        [Test]
        public void TwoThristersTryingToDrink_OnlyFirstDrinks()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionDrinks.Add(testEntity);
            players.Add(testEntity);
            thristers.Add(testEntity).DrinkSpeed = 1;
            thristers.Get(testEntity).MaxThristLevel = 1;

            var testEntity2 = world.NewEntity();
            positions.Add(testEntity2).Position = new System.Numerics.Vector2(10, 10);
            decisionDrinks.Add(testEntity2);
            players.Add(testEntity2);
            thristers.Add(testEntity2).DrinkSpeed = 1;
            thristers.Get(testEntity).MaxThristLevel = 1;

            var water = world.NewEntity();
            positions.Add(water).Position = new System.Numerics.Vector2(10, 10);
            drinkables.Add(water).CurrentAmount = 5;
            players.Add(water);
            availabilities.Add(water).MaxNumberOfUsers = 1;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(water, holders.Get(testEntity).OccupiedEntity);
            Assert.AreEqual(false, holders.Has(testEntity2));
            Assert.AreEqual(4.9f, drinkables.Get(water).CurrentAmount, 0.0001f);
        }

        [Test]
        public void WaterAppearedInProcess_Drink()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(10, 10);
            decisionDrinks.Add(testEntity);
            players.Add(testEntity);
            thristers.Add(testEntity).DrinkSpeed = 1;
            thristers.Get(testEntity).MaxThristLevel = 1;

            var water = world.NewEntity();
            positions.Add(water).Position = new System.Numerics.Vector2(10, 10);
            players.Add(water);

            sharedData.delta = 0.1f;
            systems.Run();
            drinkables.Add(water).CurrentAmount = 10;
            systems.Run();

            Assert.AreEqual(0.1f, thristers.Get(testEntity).CurrentThristing);
            Assert.AreEqual(9.9f, drinkables.Get(water).CurrentAmount);
        }
    }
}
