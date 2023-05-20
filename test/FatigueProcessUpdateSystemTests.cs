namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using NUnit.Framework;

    [TestFixture]
    public class FatigueProcessUpdateSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<FatigueComponent> fatigues;
        private EcsPool<PersonDecisionBuildComponent> decisionBuilds;
        private EcsPool<AvailabilityHolderComponent> holders;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new FatigueProcessUpdateSystem());
            systems.Init();

            fatigues = world.GetPool<FatigueComponent>();
            decisionBuilds = world.GetPool<PersonDecisionBuildComponent>();
            holders = world.GetPool<AvailabilityHolderComponent>();
        }

        [Test]
        public void RegularProcess_FatigueChangedNormally()
        {
            var testEntity = world.NewEntity();
            fatigues.Add(testEntity).CurrentFatigue = 2;
            fatigues.Get(testEntity).FatigueSpeed = 2;
            fatigues.Get(testEntity).FatigueBuilderSpeed = 3;

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(2.2f, fatigues.Get(testEntity).CurrentFatigue, 0.001f);
        }

        [Test]
        public void BuildingProcess_FatigueChangedFprBuilding()
        {
            var testEntity = world.NewEntity();
            fatigues.Add(testEntity).CurrentFatigue = 2;
            fatigues.Get(testEntity).FatigueSpeed = 2;
            fatigues.Get(testEntity).FatigueBuilderSpeed = 3;
            holders.Add(testEntity);
            decisionBuilds.Add(testEntity);

            sharedData.delta = 0.1f;
            systems.Run();

            Assert.AreEqual(2.3f, fatigues.Get(testEntity).CurrentFatigue, 0.001f);
        }
    }
}
