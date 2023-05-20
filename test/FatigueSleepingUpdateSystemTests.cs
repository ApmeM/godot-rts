namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class FatigueSleepingUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<FatigueComponent> fatigues;
        private EcsPool<NotificationComponent> notifications;
        private EcsPool<FatigueSleepComponent> sleeps;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new FatigueSleepingUpdateSystem());
            systems.Init();

            fatigues = world.GetPool<FatigueComponent>();
            sleeps = world.GetPool<FatigueSleepComponent>();
        }

        [Test]
        public void SleepInProgress_FatigueDecreased()
        {
            var testEntity = world.NewEntity();
            fatigues.Add(testEntity).CurrentFatigue = 2;
            sleeps.Add(testEntity).RestSpeed = 2;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(1.8f, fatigues.Get(testEntity).CurrentFatigue, 0.0001f);
        }

        [Test]
        public void SleepInProgress_DoNotWakeUp()
        {
            var testEntity = world.NewEntity();
            fatigues.Add(testEntity).CurrentFatigue = 2;
            sleeps.Add(testEntity).RestSpeed = 2;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(true, sleeps.Has(testEntity));
        }

        [Test]
        public void SleepFinished_WakeUp()
        {
            var testEntity = world.NewEntity();
            fatigues.Add(testEntity).CurrentFatigue = 0.1f;
            sleeps.Add(testEntity).RestSpeed = 2;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(0, fatigues.Get(testEntity).CurrentFatigue);
            Assert.AreEqual(false, sleeps.Has(testEntity));
        }
    }
}
