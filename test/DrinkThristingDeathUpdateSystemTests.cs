namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class DrinkThristingDeathUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<DrinkThristingComponent> thristings;
        private EcsPool<NotificationComponent> notifications;
        private EcsPool<DeadComponent> deads;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new DrinkThristingDeathUpdateSystem());
            systems.Init();

            thristings = world.GetPool<DrinkThristingComponent>();
            notifications = world.GetPool<NotificationComponent>();
            deads = world.GetPool<DeadComponent>();
        }

        [Test]
        public void ThristingOver_EntityDead()
        {
            var testEntity = world.NewEntity();
            thristings.Add(testEntity).CurrentThristing = -1;

            var notificationEntity = world.NewEntity();
            notifications.Add(notificationEntity);

            systems.Run();

            Assert.AreEqual(true, deads.Has(testEntity));
        }

        [Test]
        public void ThristingOver_NotificationSet()
        {
            var testEntity = world.NewEntity();
            thristings.Add(testEntity).CurrentThristing = -1;

            var notificationEntity = world.NewEntity();
            notifications.Add(notificationEntity);

            systems.Run();

            Assert.AreEqual(true, notifications.Get(notificationEntity).Notification == Notifications.ThristingDead);
        }

        [Test]
        public void ThristingNotOver_EntityDead()
        {
            var testEntity = world.NewEntity();
            thristings.Add(testEntity).CurrentThristing = 1;

            var notificationEntity = world.NewEntity();
            notifications.Add(notificationEntity);

            systems.Run();

            Assert.AreEqual(false, deads.Has(testEntity));
        }
    }
}
