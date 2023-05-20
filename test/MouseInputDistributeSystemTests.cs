namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class MouseInputDistributeSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<MouseInputComponent> input;
        private EcsPool<MouseInputDistributionComponent> distributions;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new MouseInputDistributeSystem());
            systems.Init();

            input = world.GetPool<MouseInputComponent>();
            distributions = world.GetPool<MouseInputDistributionComponent>();
        }

        [Test]
        public void MouseValueExists_Distribute()
        {
            var testEntity = world.NewEntity();
            input.Add(testEntity);

            var toDistributeEntity = world.NewEntity();
            distributions.Add(toDistributeEntity).MousePosition = new System.Numerics.Vector2(1, 1);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(1, 1), input.Get(testEntity).MousePosition);
        }

        [Test]
        public void MultipleSources_Fail()
        {
            var testEntity = world.NewEntity();
            input.Add(testEntity);

            var toDistributeEntity1 = world.NewEntity();
            distributions.Add(toDistributeEntity1);
            var toDistributeEntity2 = world.NewEntity();
            distributions.Add(toDistributeEntity2);

            Assert.Catch(() => systems.Run());
        }
    }
}
