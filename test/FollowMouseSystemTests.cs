namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class FollowMouseSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<MouseInputComponent> input;
        private EcsPool<FollowMouseComponent> follows;
        private EcsPool<PositionComponent> positions;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new FollowMouseUpdateSystem());
            systems.Init();

            input = world.GetPool<MouseInputComponent>();
            follows = world.GetPool<FollowMouseComponent>();
            positions = world.GetPool<PositionComponent>();
        }

        [Test]
        public void MouseValueExists_Distribute()
        {
            var testEntity = world.NewEntity();
            input.Add(testEntity).MousePosition = new System.Numerics.Vector2(5, 5);
            follows.Add(testEntity);
            positions.Add(testEntity);

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(5, 5), positions.Get(testEntity).Position);
        }
    }
}
