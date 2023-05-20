namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using System.Linq.Struct;
    using NUnit.Framework;

    [TestFixture]
    public class SelectPositionMouseSystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<SelectPositionMouseComponent> selectPosition;
        private EcsPool<MouseInputComponent> inputs;
        private EcsPool<EntityTypeComponent> types;
        private EcsPool<PositionComponent> positions;
        private EcsPool<PlayerComponent> players;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new SelectPositionMouseSystem());
            systems.Init();

            selectPosition = world.GetPool<SelectPositionMouseComponent>();
            inputs = world.GetPool<MouseInputComponent>();
            types = world.GetPool<EntityTypeComponent>();
            positions = world.GetPool<PositionComponent>();
            players = world.GetPool<PlayerComponent>();
        }

        [Test]
        public void LocationSelected_ReplaceWithRealEntity()
        {
            var testEntity = world.NewEntity();
            selectPosition.Add(testEntity);
            inputs.Add(testEntity).MouseButtons = (int)MouseInputComponent.ButtonList.Left;
            types.Add(testEntity).EntityType = EntityTypeComponent.EntityTypes.House;
            positions.Add(testEntity).Position = new System.Numerics.Vector2(2, 3);
            players.Add(testEntity).PlayerId = 1;

            systems.Run();

            var entities = world.Filter().Inc<EntityTypeComponent>().End();

            var entity = entities.Build().Single();

            Assert.AreNotEqual(testEntity, entity);
            Assert.AreEqual(new System.Numerics.Vector2(2, 3), positions.Get(entity).Position);
            Assert.AreEqual(EntityTypeComponent.EntityTypes.House, types.Get(entity).EntityType);
        }
    }
}
