namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class RemoveDeadUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<DeadComponent> deads;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new RemoveDeadUpdateSystem());
            systems.Init();

            deads = world.GetPool<DeadComponent>();
        }

        [Test]
        public void EntityDead_EntityRemoved()
        {
            var testEntity = world.NewEntity();
            deads.Add(testEntity);

            Assert.AreEqual(1, world.EntitiesCount());

            systems.Run();

            Assert.AreEqual(0, world.EntitiesCount());
        }
    }
}
