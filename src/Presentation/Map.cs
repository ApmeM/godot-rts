using GodotAnalysers;
using Godot;
using LocomotorECS;

[SceneReference("Map.tscn")]
public partial class Map
{
    public GameContext context;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public Map()
    {
        this.context = new GameContext(this.WorldToMap, (map) => this.MapToWorld(map));
        this.el = new EntityList();

        this.esl = new EntitySystemList(el);
        this.esl.Add(new DrinkRegenerationUpdateSystem());
        this.esl.Add(new PersonUpdateSystem());
        this.esl.Add(new PositionUpdateSystem(this.context));
        this.esl.Add(new MovingUpdateSystem(this.context));
        this.esl.Add(new ThristingUpdateSystem());

        this.render_esl = new EntitySystemList(el);
        this.render_esl.Add(new Node2DPositionRenderSystem());
        this.render_esl.Add(new Node2DDyingRenderSystem());

        // ToDo: this should be some undestructable mountains.
        const int size = 25;

        context.AddPosition(new PositionComponent()
        {
            Position = new Vector2(0, 0),
            BlockingCells = new Vector2[] { Vector2.Zero }
        });
        context.AddPosition(new PositionComponent()
        {
            Position = new Vector2(size * this.CellSize.x, 0),
            BlockingCells = new Vector2[] { Vector2.Zero }
        });
        context.AddPosition(new PositionComponent()
        {
            Position = new Vector2(0, size * this.CellSize.y),
            BlockingCells = new Vector2[] { Vector2.Zero }
        });
        context.AddPosition(new PositionComponent()
        {
            Position = new Vector2(size * this.CellSize.x, size * this.CellSize.y),
            BlockingCells = new Vector2[] { Vector2.Zero }
        });
        for (var i = 1; i < size; i++)
        {
            context.AddPosition(new PositionComponent()
            {
                Position = new Vector2(0, i * this.CellSize.y),
                BlockingCells = new Vector2[] { Vector2.Zero }
            });
            context.AddPosition(new PositionComponent()
            {
                Position = new Vector2(i * this.CellSize.x, 0),
                BlockingCells = new Vector2[] { Vector2.Zero }
            });
            context.AddPosition(new PositionComponent()
            {
                Position = new Vector2(size * this.CellSize.x, i * this.CellSize.y),
                BlockingCells = new Vector2[] { Vector2.Zero }
            });
            context.AddPosition(new PositionComponent()
            {
                Position = new Vector2(i * this.CellSize.x, size * this.CellSize.y),
                BlockingCells = new Vector2[] { Vector2.Zero }
            });
        }
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
