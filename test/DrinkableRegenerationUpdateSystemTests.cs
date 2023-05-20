namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class DrinkableRegenerationUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<DrinkableComponent> drinks;
        private EcsPool<DrinkableRegenerationComponent> regenerations;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new DrinkableRegenerationUpdateSystem());
            systems.Init();

            drinks = world.GetPool<DrinkableComponent>();
            regenerations = world.GetPool<DrinkableRegenerationComponent>();
        }

        [Test]
        public void NormalRegeneration_valueAdded()
        {
            var testEntity = world.NewEntity();
            drinks.Add(testEntity).CurrentAmount = 10;
            regenerations.Add(testEntity).Regeneration = 1;
            regenerations.Get(testEntity).MaxAmount = 20;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(10.1f, drinks.Get(testEntity).CurrentAmount, 0.001f);
        }

        [Test]
        public void AlreadyMax_ValueNotAdded()
        {
            var testEntity = world.NewEntity();
            drinks.Add(testEntity).CurrentAmount = 20;
            regenerations.Add(testEntity).Regeneration = 1;
            regenerations.Get(testEntity).MaxAmount = 20;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(20, drinks.Get(testEntity).CurrentAmount, 0.001f);
        }

        [Test]
        public void AddedMorethenMax_ValueAddedToMax()
        {
            var testEntity = world.NewEntity();
            drinks.Add(testEntity).CurrentAmount = 19.9f;
            regenerations.Add(testEntity).Regeneration = 10;
            regenerations.Get(testEntity).MaxAmount = 20;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(20, drinks.Get(testEntity).CurrentAmount, 0.001f);
        }

        [Test]
        public void OneWellAlreadyFull_SecondStillShouldRegenerate()
        {
            var testEntity1 = world.NewEntity();
            drinks.Add(testEntity1).CurrentAmount = 20;
            regenerations.Add(testEntity1).Regeneration = 10;
            regenerations.Get(testEntity1).MaxAmount = 20;

            var testEntity2 = world.NewEntity();
            drinks.Add(testEntity2).CurrentAmount = 10;
            regenerations.Add(testEntity2).Regeneration = 10;
            regenerations.Get(testEntity2).MaxAmount = 20;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(11, drinks.Get(testEntity2).CurrentAmount, 0.001f);
        }
    }
}
