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
            notifications.Add(testEntity).Notification = Notifications.SleepingOnTheGround;

            Assert.AreEqual(Notifications.SleepingOnTheGround, notifications.Get(testEntity).Notification);

            systems.Run();

            Assert.AreEqual(Notifications.None, notifications.Get(testEntity).Notification);
        }
    }
}
