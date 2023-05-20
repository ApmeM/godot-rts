namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class FatigueToSleepUpdateSystemTests
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
            systems.Add(new FatigueToSleepUpdateSystem());
            systems.Init();

            fatigues = world.GetPool<FatigueComponent>();
            notifications = world.GetPool<NotificationComponent>();
            sleeps = world.GetPool<FatigueSleepComponent>();
        }

        [Test]
        public void FallAsleep_SleepNotInHouse()
        {
            var testEntity = world.NewEntity();
            fatigues.Add(testEntity).MaxFatigue = 1;
            fatigues.Get(testEntity).CurrentFatigue = 2;

            var notificationEntity = world.NewEntity();
            notifications.Add(notificationEntity);

            systems.Run();

            Assert.AreEqual(false, sleeps.Get(testEntity).InHouse);
        }


        [Test]
        public void FallAsleep_SetDefaultRestSpeed()
        {
            var testEntity = world.NewEntity();
            fatigues.Add(testEntity).MaxFatigue = 1;
            fatigues.Get(testEntity).CurrentFatigue = 2;
            fatigues.Get(testEntity).DefaultRest = 21;

            var notificationEntity = world.NewEntity();
            notifications.Add(notificationEntity);

            systems.Run();

            Assert.AreEqual(21, sleeps.Get(testEntity).RestSpeed);
        }

        [Test]
        public void FallAsleep_NotificationSent()
        {
            var testEntity = world.NewEntity();
            fatigues.Add(testEntity).MaxFatigue = 1;
            fatigues.Get(testEntity).CurrentFatigue = 2;

            var notificationEntity = world.NewEntity();
            notifications.Add(notificationEntity);

            systems.Run();

            Assert.AreEqual(true, notifications.Get(notificationEntity).SleepingOnTheGround);
        }

        [Test]
        public void NotTiredEnough_DoNothing()
        {
            var testEntity = world.NewEntity();
            fatigues.Add(testEntity).MaxFatigue = 1;
            fatigues.Get(testEntity).CurrentFatigue = 0;

            var notificationEntity = world.NewEntity();
            notifications.Add(notificationEntity);

            systems.Run();

            Assert.AreEqual(false, sleeps.Has(testEntity));
        }
    }
}
