namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class NotificationsCleanupUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<NotificationComponent> notifications;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new NotificationsCleanupUpdateSystem());
            systems.Init();

            notifications = world.GetPool<NotificationComponent>();
        }

        [Test]
        public void EntityDead_EntityRemoved()
        {
            var testEntity = world.NewEntity();
            notifications.Add(testEntity).SleepingOnTheGround = true;
            notifications.Get(testEntity).ThristingDead = true;

            Assert.AreEqual(true, notifications.Get(testEntity).SleepingOnTheGround);
            Assert.AreEqual(true, notifications.Get(testEntity).ThristingDead);

            systems.Run();

            Assert.AreEqual(false, notifications.Get(testEntity).SleepingOnTheGround);
            Assert.AreEqual(false, notifications.Get(testEntity).ThristingDead);
        }
    }
}
