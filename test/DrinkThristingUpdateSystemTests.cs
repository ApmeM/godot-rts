namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class DrinkThristingUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<DrinkThristingComponent> thristings;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new DrinkThristingUpdateSystem());
            systems.Init();

            thristings = world.GetPool<DrinkThristingComponent>();
        }

        [Test]
        public void ThristingDecreased()
        {
            var testEntity = world.NewEntity();
            thristings.Add(testEntity).CurrentThristing = 1;
            thristings.Get(testEntity).ThristSpeed = 1;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(0.9f, thristings.Get(testEntity).CurrentThristing, 0.001f);
        }

        [Test]
        public void ThristingDecreased_EvenNegative()
        {
            var testEntity = world.NewEntity();
            thristings.Add(testEntity).CurrentThristing = -1;
            thristings.Get(testEntity).ThristSpeed = 1;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(-1.1f, thristings.Get(testEntity).CurrentThristing, 0.001f);
        }
    }
}
