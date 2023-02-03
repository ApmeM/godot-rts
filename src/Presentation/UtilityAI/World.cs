using System;
using Godot;
using GodotRts.Presentation.Utils;
using LocomotorECS;

public class World
{
    public World(Map map)
    {
        this.map = map;

        this.context = new GameContext(map.WorldToMap, (point) => map.MapToWorld(point));
        this.el = new EntityList();

        this.input_esl = new EntitySystemList(el);
        this.input_esl.Add(new MouseInputSystem(map));

        this.esl = new EntitySystemList(el);
        this.esl.Add(new BuildMoveUpdateSystem());
        this.esl.Add(new BuildProcessUpdateSystem());
        this.esl.Add(new DrinkableRegenerationUpdateSystem());
        this.esl.Add(new DrinkMoveUpdateSystem());
        this.esl.Add(new DrinkProcessUpdateSystem());
        this.esl.Add(new DrinkThristingDeathUpdateSystem());
        this.esl.Add(new DrinkThristingUpdateSystem());
        this.esl.Add(new FatigueProcessUpdateSystem());
        this.esl.Add(new FatigueMoveUpdateSystem());
        this.esl.Add(new FatigueSleepingUpdateSystem());
        this.esl.Add(new FatigueSleepThristingSyncUpdateSystem());
        this.esl.Add(new FatigueToSleepUpdateSystem());
        this.esl.Add(new MovingUpdateSystem(this.context));
        this.esl.Add(new PersonDecisionBuildAvailabilitySyncUpdateSystem());
        this.esl.Add(new PersonDecisionDrinkAvailabilitySyncUpdateSystem());
        this.esl.Add(new PersonDecisionUpdateSystem());
        this.esl.Add(new PositionUpdateSystem(this.context));
        this.esl.Add(new ReproductionUpdateSystem(el));
        this.esl.Add(new SelectingEntitySystem());
        this.esl.Add(new WalkingUpdateSystem());

        this.render_esl = new EntitySystemList(el);
        this.render_esl.Add(new EntityTypeNode2DRenderSystem(map));
        this.render_esl.Add(new Node2DPositionRenderSystem());
        this.render_esl.Add(new Node2DDyingRenderSystem());
    }

    public void BuildFromDesignTime()
    {
        foreach (Node2D child in map.GetChildren())
        {
            Entity entity;
            switch (child)
            {
                case ArtificialWell artificialWell:
                    entity = Entities.BuildArificialWell();
                    break;
                case House house:
                    entity = Entities.BuildHouse();
                    break;
                case Person person:
                    entity = Entities.BuildPerson();
                    break;
                case Tree tree:
                    entity = Entities.BuildTree();
                    break;
                case Well well:
                    entity = Entities.BuildWell();
                    break;
                default:
                    entity = null;
                    break;
            }

            entity.GetComponent<PositionComponent>().Position = child.Position;
            this.el.Add(entity);
        }
    }

    public GameContext context;
    public readonly EntityList el;
    public readonly EntitySystemList esl;
    public readonly EntitySystemList render_esl;
    public readonly EntitySystemList input_esl;
    private readonly TileMap map;

    public void Process(float delta)
    {
        input_esl.NotifyDoAction(delta);
        esl.NotifyDoAction(delta);
        render_esl.NotifyDoAction(delta);
        el.CommitChanges();
    }

    public void BuildFence(int size, float stepX, float stepY)
    {
        Entity e;
        e = Entities.BuildTree();
        e.GetComponent<PositionComponent>().Position = new Vector2(0, 0);
        this.el.Add(e);
        e = Entities.BuildTree();
        e.GetComponent<PositionComponent>().Position = new Vector2(size * stepX, 0);
        this.el.Add(e);
        e = Entities.BuildTree();
        e.GetComponent<PositionComponent>().Position = new Vector2(0, size * stepY);
        this.el.Add(e);
        e = Entities.BuildTree();
        e.GetComponent<PositionComponent>().Position = new Vector2(size * stepX, size * stepY);
        this.el.Add(e);

        for (var i = 1; i < size; i++)
        {
            e = Entities.BuildTree();
            e.GetComponent<PositionComponent>().Position = new Vector2(0, i * stepY);
            this.el.Add(e);
            e = Entities.BuildTree();
            e.GetComponent<PositionComponent>().Position = new Vector2(i * stepX, 0);
            this.el.Add(e);
            e = Entities.BuildTree();
            e.GetComponent<PositionComponent>().Position = new Vector2(size * stepX, i * stepY);
            this.el.Add(e);
            e = Entities.BuildTree();
            e.GetComponent<PositionComponent>().Position = new Vector2(i * stepX, size * stepY);
            this.el.Add(e);
        }
    }
}
