using System;
using System.Linq;
using BrainAI.Pathfinding;
using Godot;
using GodotAnalysers;
using BrainAI.AI.UtilityAI;
using System.Collections.Generic;

[SceneReference("Person.tscn")]
public partial class Person
{
    public class Context : TileMapObject.Context, IPrintActionContext, IMoveActionContext, IDrinkActionContext, IBuildActionContext
    {
        public IBuildItemActionContext Build { get; set; }
        public float BuildSpeed { get; set; } = 1;

        public IDrinkFromActionContext Well { get; set; }


        public string CurrentActionName { get; set; }

        public float Delta { get; set; }

        public bool IsDead { get; set; }

        public float ThinkTimeout { get; set; }

        public List<Vector2> Path { get; set; }

        public float ThristThreshold { get; set; } = 50;

        public float CurrentThristLevel { get; set; } = 100;

        public float MaxThristLevel { get; set; } = 100;

        public float MoveSpeed { get; set; } = 128;

        public float ThristSpeed { get; set; } = 3f;

        public float DrinkSpeed { get; set; } = 50f;

        public void Drink(float amount)
        {
            this.CurrentThristLevel += amount;
            if (Math.Abs(this.MaxThristLevel - this.CurrentThristLevel) < 0.1f)
            {
                this.Well = null;
            }
        }

        public void Tick(float delta)
        {
            this.CurrentThristLevel -= this.ThristSpeed * delta;
            this.Delta = delta;
        }
    }

    public Random r = new Random();

    private Context myContext => (Context)this.context;

    private UtilityAI<Context> ai;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.myContext.Tick(delta);
        this.ai.Tick();

        if (this.myContext.IsDead)
        {
            this.QueueFree();
        }

        this.label.Text = this.myContext.CurrentActionName + " " + this.myContext.CurrentThristLevel.ToString("#");
    }

    public override void InitContext(Map.Context mapContext)
    {
        this.context = this.context ?? new Context();
        base.InitContext(mapContext);
        this.ai = new UtilityAI<Context>(this.myContext, InitAI());
    }

    public Reasoner<Context> InitAI()
    {
        var reasoner = new FirstScoreReasoner<Context>(1);

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.CurrentThristLevel <= 0 ? 1 : 0),
                new FixedAppraisal<Context>(1000)
                ),
                new PrintAction<Context>("Dying"),
                new ActionAction<Context>(a =>
                {
                    a.IsDead = true;
                }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.CurrentThristLevel < a.ThristThreshold ? 1 : 0),
                new ActionAppraisal<Context>(a => a.Well == null ? 1 : 0)
                ),
                new PrintAction<Context>("Looking for water"),
                new ActionAction<Context>(a =>
                {
                    a.Well = a.MapContext.KnownPositions.Keys.OfType<IDrinkFromActionContext>().Where(b => b.IsDrinkable).OrderBy(b => (b.Position - a.Position).LengthSquared()).First();
                    var personMap = a.MapContext.WorldToMap(this.Position);
                    var wellMap = a.MapContext.WorldToMap(a.Well.Position);
                    a.Path = AStarPathfinder.Search(a.MapContext, personMap, wellMap);
                }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.Well != null ? 1 : 0),
                new ActionAppraisal<Context>(a => a.Well.Position != a.Position ? 1 : 0)
                ),
                new PrintAction<Context>("Moving for water"),
                new MoveAction<Context>());

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.Well != null ? 1 : 0),
                new ActionAppraisal<Context>(a => a.Well.Position == a.Position ? 1 : 0)
                ),
                new PrintAction<Context>("Drinking for water"),
                new DrinkAction<Context>());

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.MapContext.KnownPositions.Keys.OfType<Construction.Context>().Any(b => b.BuildHP != b.MaxHP) ? 1 : 0),
                new ActionAppraisal<Context>(a => a.Build == null ? 1 : 0)
                ),
                new PrintAction<Context>("Going to build"),
                new ActionAction<Context>(
                    a =>
                    {
                        a.Build = a.MapContext.KnownPositions.Keys.OfType<Construction.Context>().First(b => b.BuildHP != b.MaxHP);

                        var targetMap = a.MapContext.WorldToMap(a.Build.Position);
                        var personMap = a.MapContext.WorldToMap(this.Position);
                        a.Path = AStarPathfinder.Search(a.MapContext, personMap, targetMap);
                    }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.Build != null ? 1 : 0),
                new ActionAppraisal<Context>(a => a.Build.Position != a.Position ? 1 : 0)
                ),
                new PrintAction<Context>("Moving for build"),
                new MoveAction<Context>());

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.Build != null ? 1 : 0),
                new ActionAppraisal<Context>(a => a.Build.Position == a.Position ? 1 : 0)
                ),
                new PrintAction<Context>("Building for build"),
                new BuildAction<Context>());

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.Path == null || !a.Path.Any() ? 1 : 0)
                ),
                new PrintAction<Context>("Thinking"),
                new ActionAction<Context>(
                    a => { a.ThinkTimeout = this.r.Next(3); },
                    a =>
                    {
                        a.ThinkTimeout -= a.Delta;
                        if (a.ThinkTimeout > 0)
                        {
                            return;
                        }

                        var targetMap = new Vector2(r.Next(25), r.Next(25));
                        var personMap = a.MapContext.WorldToMap(this.Position);
                        a.Path = AStarPathfinder.Search(a.MapContext, personMap, targetMap);
                    },
                    a => { }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new FixedAppraisal<Context>(1),
                new ActionAppraisal<Context>(a => a.Path != null && a.Path.Any() ? 1 : 0)
                ),
                new PrintAction<Context>("Walking"),
                new MoveAction<Context>());

        return reasoner;
    }
}

