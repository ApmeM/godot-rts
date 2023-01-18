using System;
using System.Linq;
using Godot;
using LocomotorECS;

public class PersonUpdateSystem : MatcherEntitySystem
{
    private MatcherEntityList waterSources;
    private MatcherEntityList buildSources;
    private Random r = new Random();

    public PersonUpdateSystem() : base(new Matcher().All<PersonComponent>().All<MovingComponent>().All<PositionComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var position = entity.GetComponent<PositionComponent>();
        var moving = entity.GetComponent<MovingComponent>();

        var thristing = entity.GetComponent<ThristingComponent>();
        if (thristing != null)
        {
            var closestSource = waterSources.Entities
                    .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                    .FirstOrDefault();
            if (closestSource != null)
            {
                var closestWater = closestSource.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;

                if (position.Position != closestWater && thristing.CurrentThristLevel < thristing.ThristThreshold)
                {
                    entity.GetComponent<PrintComponent>().Text = "Going to drink";
                    moving.PathTarget = closestWater;
                    return;
                }

                if (position.Position == closestWater && thristing.CurrentThristLevel < thristing.MaxThristLevel)
                {
                    var drinkable = closestSource.GetComponent<DrinkableComponent>();
                    var toDrink = Mathf.Min(Mathf.Min(thristing.DrinkSpeed * delta, thristing.MaxThristLevel - thristing.CurrentThristLevel), drinkable.CurrentAmount);
                    drinkable.CurrentAmount -= toDrink;
                    thristing.CurrentThristLevel += toDrink;

                    if (thristing.CurrentThristLevel < thristing.MaxThristLevel)
                    {
                        entity.GetComponent<PrintComponent>().Text = "Drinking";
                        return;
                    }
                }
            }
        }

        var builder = entity.GetComponent<BuilderComponent>();
        if (builder != null)
        {
            var closestSource = buildSources.Entities
                    .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                    .FirstOrDefault();
            if (closestSource != null)
            {
                var closestConstruction = closestSource.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;

                if (position.Position != closestConstruction)
                {
                    entity.GetComponent<PrintComponent>().Text = "Going to construction";
                    moving.PathTarget = closestConstruction;
                    return;
                }

                if (position.Position == closestConstruction && thristing.CurrentThristLevel < thristing.MaxThristLevel)
                {
                    entity.GetComponent<PrintComponent>().Text = "Building";
                    var construction = closestSource.GetComponent<ConstructionComponent>();

                    if (construction.BuildProgress >= 1)
                    {
                        entity.GetComponent<PrintComponent>().Text = "Built";
                        construction.ConstructionDone?.Invoke(closestSource);
                        construction.ConstructionDone = null;
                        closestSource.RemoveComponent<ConstructionComponent>();
                        return;
                    }

                    var hpToPct = 0.01f;
                    var hp = closestSource.GetComponent<HPComponent>();
                    if (hp != null)
                    {
                        hpToPct = 1f / hp.MaxHP;
                    }
                    var buildProgress = builder.BuildSpeed * hpToPct;
                    construction.BuildProgress += buildProgress;
                    if (hp != null)
                    {
                        hp.HP += builder.BuildSpeed;
                        if (hp.HP > hp.MaxHP)
                        {
                            hp.HP = hp.MaxHP;
                        }
                    }

                    return;
                }
            }
        }

        entity.GetComponent<PrintComponent>().Text = "Walking";
        if (moving.PathTarget == Godot.Vector2.Inf)
        {
            moving.PathTarget = position.Position + new Vector2(r.Next(250) - 125, r.Next(250) - 125);
        }
    }

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.waterSources = new MatcherEntityList(entityList, new Matcher().All<DrinkableComponent>().All<PositionComponent>());
        this.buildSources = new MatcherEntityList(entityList, new Matcher().All<ConstructionComponent>().All<PositionComponent>());
        return base.FilterEntityList(entityList);
    }
}
