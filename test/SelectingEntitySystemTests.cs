namespace GodotRts.Tests
{
    using Leopotam.EcsLite;
    using System.Linq.Struct;
    using NUnit.Framework;

    [TestFixture]
    public class SelectingEntitySystemTests
    {
        EcsWorld world;
        EcsSystems systems;
        World.SharedData sharedData;
        private EcsPool<SelectableComponent> selectables;
        private EcsPool<MouseInputComponent> inputs;
        private EcsPool<PositionComponent> positions;
        private EcsPool<SelectedComponent> selecteds;

        [SetUp]
        public void Setup()
        {
            sharedData = new World.SharedData();
            sharedData.stepX = 1;
            sharedData.stepY = 1;
            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            systems.Add(new SelectingEntitySystem());
            systems.Init();

            selectables = world.GetPool<SelectableComponent>();
            inputs = world.GetPool<MouseInputComponent>();
            positions = world.GetPool<PositionComponent>();
            selecteds = world.GetPool<SelectedComponent>();
        }

        [Test]
        public void SelectingAlreadySelected_NohingChanged()
        {
            var testEntity = world.NewEntity();
            selectables.Add(testEntity);
            inputs.Add(testEntity).JustPressedButtins = (int)MouseInputComponent.ButtonList.Left;
            inputs.Get(testEntity).MousePosition = new System.Numerics.Vector2(2, 3);
            positions.Add(testEntity).Position = new System.Numerics.Vector2(2, 3);
            selecteds.Add(testEntity);

            systems.Run();

            Assert.AreEqual(true, selecteds.Has(testEntity));
        }

        [Test]
        public void SelectingNotSelected_Selected()
        {
            var testEntity = world.NewEntity();
            selectables.Add(testEntity);
            inputs.Add(testEntity).JustPressedButtins = (int)MouseInputComponent.ButtonList.Left;
            inputs.Get(testEntity).MousePosition = new System.Numerics.Vector2(2, 3);
            positions.Add(testEntity).Position = new System.Numerics.Vector2(2, 3);

            systems.Run();

            Assert.AreEqual(true, selecteds.Has(testEntity));
        }

        [Test]
        public void SelectingClickingFarFromSelected_Unselect()
        {
            var testEntity = world.NewEntity();
            selectables.Add(testEntity);
            inputs.Add(testEntity).JustPressedButtins = (int)MouseInputComponent.ButtonList.Left;
            inputs.Get(testEntity).MousePosition = new System.Numerics.Vector2(3, 4);
            positions.Add(testEntity).Position = new System.Numerics.Vector2(2, 3);
            selecteds.Add(testEntity);

            systems.Run();

            Assert.AreEqual(false, selecteds.Has(testEntity));
        }

        [Test]
        public void Unselectingunselected_NothingChanged()
        {
            var testEntity = world.NewEntity();
            selectables.Add(testEntity);
            inputs.Add(testEntity).JustPressedButtins = (int)MouseInputComponent.ButtonList.Left;
            inputs.Get(testEntity).MousePosition = new System.Numerics.Vector2(200, 300);
            positions.Add(testEntity).Position = new System.Numerics.Vector2(2, 3);

            systems.Run();

            Assert.AreEqual(false, selecteds.Has(testEntity));
        }
    }
}
