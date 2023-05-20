namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Numerics;

    [TestFixture]
    public class MovingUpdateSystemTests
    {
        public class FakePathfinder : IPathFinder
        {
            public List<Vector2> FindPath(Vector2 from, Vector2 to)
            {
                return new List<Vector2> { from, to };
            }
        }

        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        IPathFinder context;
        private EcsPool<PositionComponent> positions;
        private EcsPool<MovingComponent> movings;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            context = new FakePathfinder();
            systems.Add(new MovingUpdateSystem(context));
            systems.Init();

            positions = world.GetPool<PositionComponent>();
            movings = world.GetPool<MovingComponent>();
        }

        [Test]
        public void NowhereToGo_PositionNotChanged()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(2, 3);
            movings.Add(testEntity).PathTarget = System.Numerics.Vector2Ext.Inf;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(new System.Numerics.Vector2(2, 3), positions.Get(testEntity).Position);
        }

        [Test]
        public void HavePlaceToGoButCurrentPathIsDifferent_PathUpdatedToNew()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(5, 3);
            movings.Add(testEntity).MoveSpeed = 2;
            movings.Get(testEntity).PathTarget = new System.Numerics.Vector2(5, 5);
            movings.Get(testEntity).Path = new List<Vector2>{
                new System.Numerics.Vector2(5, 3)
            };

            sharedData.delta = 0.1f;
            systems.Run(); // First Run checks source point
            systems.Run(); // Second run do the real move

            Assert.AreEqual(new List<Vector2> { new System.Numerics.Vector2(5, 5) }, movings.Get(testEntity).Path);
            Assert.AreEqual(new System.Numerics.Vector2(5, 3.2f), positions.Get(testEntity).Position);
        }

        [Test]
        public void AtTargetLocation_NoNeedToGo()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(5, 5);
            movings.Add(testEntity).MoveSpeed = 2;
            movings.Get(testEntity).PathTarget = new System.Numerics.Vector2(5, 5);
            movings.Get(testEntity).Path = new List<Vector2>{
                new System.Numerics.Vector2(5, 5)
            };

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(new List<Vector2>(), movings.Get(testEntity).Path);
            Assert.AreEqual(Vector2Ext.Inf, movings.Get(testEntity).PathTarget);
            Assert.AreEqual(new System.Numerics.Vector2(5, 5), positions.Get(testEntity).Position);
        }

        [Test]
        public void AtTargetLocation_AddPointToPath()
        {
            var testEntity = world.NewEntity();
            positions.Add(testEntity).Position = new System.Numerics.Vector2(5, 5);
            movings.Add(testEntity).MoveSpeed = 2;
            movings.Get(testEntity).PathTarget = new System.Numerics.Vector2(5, 5);
            movings.Get(testEntity).Path = new List<Vector2>();

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(new List<Vector2> { new System.Numerics.Vector2(5, 5) }, movings.Get(testEntity).Path);
            Assert.AreEqual(new System.Numerics.Vector2(5, 5), movings.Get(testEntity).PathTarget);
            Assert.AreEqual(new System.Numerics.Vector2(5, 5), positions.Get(testEntity).Position);
        }
    }
}
