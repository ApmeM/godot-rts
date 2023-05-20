namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class BuildFinishUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<ConstructionComponent> constructions;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new BuildFinishUpdateSystem());
            systems.Init();

            constructions = world.GetPool<ConstructionComponent>();
        }

        [Test]
        public void ConstructionDone_MothodInvoked()
        {
            var isDone = false;
            var construction = world.NewEntity();
            constructions.Add(construction).MaxBuildProgress = 10;
            constructions.Get(construction).BuildProgress = 10;
            constructions.Get(construction).ConstructionDone = (e) => isDone = true;

            systems.Run();

            Assert.AreEqual(true, isDone);
        }
        [Test]
        public void ConstructionNotDone_MothodNotInvoked()
        {
            var isDone = false;
            var construction = world.NewEntity();
            constructions.Add(construction).MaxBuildProgress = 100;
            constructions.Get(construction).BuildProgress = 10;
            constructions.Get(construction).ConstructionDone = (e) => isDone = true;

            systems.Run();

            Assert.AreEqual(false, isDone);
        }
    }
}
