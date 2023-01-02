using System;
using System.Linq;
using Godot;
using GodotAnalysers;
using BrainAI.AI.UtilityAI;
using System.Collections.Generic;

[SceneReference("Person.tscn")]
public partial class Person
{
    public class Context : TileMapObject.Context
    {
        public float BuildSpeed { get; set; } = 1;

        public string CurrentActionName { get; set; }

        public float Delta { get; set; }

        public bool IsDead { get; set; }

        public float ThinkTimeout { get; set; }

        public List<Vector2> Path { get; set; }

        public Vector2 PathTarget { get; set; }

        public float ThristThreshold { get; set; } = 50;

        public float CurrentThristLevel { get; set; } = 100;

        public float MaxThristLevel { get; set; } = 100;

        public float MoveSpeed { get; set; } = 128;

        public float ThristSpeed { get; set; } = 3f;

        public float DrinkSpeed { get; set; } = 50f;

        public void Print(string text)
        {
            this.CurrentActionName = text;
            // Godot.GD.Print(text);
        }

        public void Drink()
        {
            var toDrink = Math.Min(this.DrinkSpeed * this.Delta, this.MaxThristLevel - this.CurrentThristLevel);
            var water = (IDrinkFromActionContext)this.MapContext.FindClosestItemByType(Map.Context.MapItemType.Water, this.Position);
            this.CurrentThristLevel += water.TryDrink(toDrink);
        }

        public void Move()
        {
            var current = this.Position;
            var destination = this.Path[0];
            var path = destination - current;
            var motion = path.Normalized() * this.MoveSpeed * this.Delta;
            if (path.LengthSquared() > motion.LengthSquared())
            {
                this.ChangePosition(current + motion);
            }
            else
            {
                this.ChangePosition(destination);
                this.Path.RemoveAt(0);
            }
        }

        public void Tick(float delta)
        {
            this.CurrentThristLevel -= this.ThristSpeed * delta;
            this.Delta = delta;
        }

        public void SetMoveTarget(Vector2 target)
        {
            if (target != PathTarget)
            {
                this.Path = this.MapContext.FindPath(this.Position, target);
                this.PathTarget = target;
            }
        }

        public void Build()
        {
            var construction = (IBuildItemActionContext)this.MapContext.FindClosestItemByType(Map.Context.MapItemType.Construction, this.Position);
            construction.Build(this.BuildSpeed * this.Delta);
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
                new ActionAppraisal<Context>(a => a.CurrentThristLevel <= 0 ? 1 : 0)
            ),
            new ActionAction<Context>(a => a.Print("Dying")),
            new ActionAction<Context>(a =>
            {
                a.IsDead = true;
            }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.CurrentThristLevel < a.ThristThreshold ? 1 : 0),
                new ActionAppraisal<Context>(a => a.MapContext.ItemByTypeExists(Map.Context.MapItemType.Water) ? 1 : 0),
                new ActionAppraisal<Context>(a => a.MapContext.FindClosestItemByType(Map.Context.MapItemType.Water, a.Position).Position != Position ? 1 : 0)
            ),
            new ActionAction<Context>(a => a.Print("Going to Drink")),
            new ActionAction<Context>(a =>
            {
                a.SetMoveTarget(a.MapContext.FindClosestItemByType(Map.Context.MapItemType.Water, a.Position).Position);
                a.Move();
            }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.CurrentThristLevel < a.MaxThristLevel - 1 ? 1 : 0),
                new ActionAppraisal<Context>(a => a.MapContext.ItemByTypeExists(Map.Context.MapItemType.Water) ? 1 : 0),
                new ActionAppraisal<Context>(a => a.MapContext.FindClosestItemByType(Map.Context.MapItemType.Water, a.Position).Position == Position ? 1 : 0)
            ),
            new ActionAction<Context>(a => a.Print("Drinking")),
            new ActionAction<Context>(a => a.Drink()));

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.MapContext.ItemByTypeExists(Map.Context.MapItemType.Construction) ? 1 : 0),
                new ActionAppraisal<Context>(a => a.MapContext.FindClosestItemByType(Map.Context.MapItemType.Construction, a.Position).Position != Position ? 1 : 0)
            ),
            new ActionAction<Context>(a => a.Print("Going to build")),
            new ActionAction<Context>(
                a =>
                {
                    a.SetMoveTarget(a.MapContext.FindClosestItemByType(Map.Context.MapItemType.Construction, a.Position).Position);
                    a.Move();
                }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.MapContext.ItemByTypeExists(Map.Context.MapItemType.Construction) ? 1 : 0),
                new ActionAppraisal<Context>(a => a.MapContext.FindClosestItemByType(Map.Context.MapItemType.Construction, a.Position).Position == Position ? 1 : 0)
            ),
            new ActionAction<Context>(a => a.Print("Building")),
            new ActionAction<Context>(a => a.Build()));

        reasoner.Add(
            new MultAppraisal<Context>(
                new ActionAppraisal<Context>(a => a.Path == null || !a.Path.Any() ? 1 : 0)
            ),
            new ActionAction<Context>(a => a.Print("Thinking")),
            new ActionAction<Context>(
                a => { a.ThinkTimeout = this.r.Next(3); },
                a =>
                {
                    a.ThinkTimeout -= a.Delta;
                    if (a.ThinkTimeout > 0)
                    {
                        return;
                    }

                    a.SetMoveTarget(this.Position + new Vector2(r.Next(250) - 125, r.Next(250) - 125));
                },
                a => { }));

        reasoner.Add(
            new MultAppraisal<Context>(
                new FixedAppraisal<Context>(1),
                new ActionAppraisal<Context>(a => a.Path != null && a.Path.Any() ? 1 : 0)
            ),
            new ActionAction<Context>(a => a.Print("Walking")),
            new ActionAction<Context>(a => a.Move()));

        return reasoner;
    }
}

