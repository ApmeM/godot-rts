namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;
    using System.Numerics;

    [TestFixture]
    public class PositionUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        GameContext context;
        private EcsPool<PositionComponent> positions;
        private EcsPool<BindToMapComponent> binds;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            context = new GameContext(a => new Vector2((int)(a.X / 2), (int)(a.Y / 2)), a => new Vector2((int)(a.X * 2), (int)(a.Y * 2)));
            systems.Add(new PositionUpdateSystem(context));
            systems.Init();

            positions = world.GetPool<PositionComponent>();
        }

        [Test]
        public void LocationSelected_PositionUpdatedInContext()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(2, 3);
            positions.Get(testEntity).BlockingCells = new[] { new System.Numerics.Vector2(0, 0) };

            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(2, 3), context.Map.Map[new Vector2(1, 1)].Position);
        }
    }
}
