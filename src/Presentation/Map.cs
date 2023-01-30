using GodotAnalysers;
using Godot;
using LocomotorECS;
using GodotRts.Presentation.Utils;

[SceneReference("Map.tscn")]
public partial class Map
{
    public GameContext context;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        // Design time elements:
        foreach (Node2D child in this.GetChildren())
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

        this.ClearChildren();

        // ToDo: this should be some undestructable mountains.
        const int size = 25;

        Entity e;
        e = Entities.BuildTree();
        e.GetComponent<PositionComponent>().Position = new Vector2(0, 0);
        el.Add(e);
        e = Entities.BuildTree();
        e.GetComponent<PositionComponent>().Position = new Vector2(size * this.CellSize.x, 0);
        el.Add(e);
        e = Entities.BuildTree();
        e.GetComponent<PositionComponent>().Position = new Vector2(0, size * this.CellSize.y);
        el.Add(e);
        e = Entities.BuildTree();
        e.GetComponent<PositionComponent>().Position = new Vector2(size * this.CellSize.x, size * this.CellSize.y);
        el.Add(e);

        for (var i = 1; i < size; i++)
        {
            e = Entities.BuildTree();
            e.GetComponent<PositionComponent>().Position = new Vector2(0, i * this.CellSize.y);
            el.Add(e);
            e = Entities.BuildTree();
            e.GetComponent<PositionComponent>().Position = new Vector2(i * this.CellSize.x, 0);
            el.Add(e);
            e = Entities.BuildTree();
            e.GetComponent<PositionComponent>().Position = new Vector2(size * this.CellSize.x, i * this.CellSize.y);
            el.Add(e);
            e = Entities.BuildTree();
            e.GetComponent<PositionComponent>().Position = new Vector2(i * this.CellSize.x, size * this.CellSize.y);
            el.Add(e);
        }
    }

    public Map()
    {
        this.context = new GameContext(this.WorldToMap, (map) => this.MapToWorld(map));
        this.el = new EntityList();

        this.render_esl = new EntitySystemList(el);
        this.render_esl.Add(new EntityTypeNode2DRenderSystem(this));
        this.render_esl.Add(new Node2DPositionRenderSystem()); // This system should be added before PositionUpdateSystem
        this.render_esl.Add(new Node2DDyingRenderSystem());

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
        this.esl.Add(new WalkingUpdateSystem());
    }

    public readonly EntityList el;
    public readonly EntitySystemList esl;
    public readonly EntitySystemList render_esl;

    public override void _Process(float delta)
    {
        base._Process(delta);

        esl.NotifyDoAction(delta);
        render_esl.NotifyDoAction(delta);

        el.CommitChanges();
    }
}
