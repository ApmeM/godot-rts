using System;
using System.Numerics;
using LocomotorECS;

public class World
{
    public World(Func<Vector2, Vector2> worldToMap, Func<Vector2, Vector2> mapToWorld)
    {
        this.context = new GameContext(worldToMap, mapToWorld);
        this.el = new EntityList();

        var constructionSource = new EntityLookup<int>(
            new MatcherEntityList(this.el, new Matcher().All<ConstructionComponent>().All<PositionComponent>()),
            e => e.GetComponent<PlayerComponent>()?.PlayerId ?? 0
        );
        var restSources = new EntityLookup<int>(
            new MatcherEntityList(el, new Matcher().All<RestComponent>().All<PositionComponent>()),
            e => e.GetComponent<PlayerComponent>()?.PlayerId ?? 0
        );
        var waterSources = new EntityLookup<int>(
            new MatcherEntityList(el, new Matcher().All<DrinkableComponent>().All<PositionComponent>()),
            e => e.GetComponent<PlayerComponent>()?.PlayerId ?? 0
        );

        this.input_esl = new EntitySystemList(el);

        this.esl = new EntitySystemList(el);
        this.esl.Add(new BuildMoveUpdateSystem(constructionSource));
        this.esl.Add(new BuildProcessUpdateSystem(constructionSource));
        this.esl.Add(new DrinkableRegenerationUpdateSystem());
        this.esl.Add(new DrinkMoveUpdateSystem(waterSources));
        this.esl.Add(new DrinkProcessUpdateSystem(waterSources));
        this.esl.Add(new DrinkThristingDeathUpdateSystem());
        this.esl.Add(new DrinkThristingUpdateSystem());
        this.esl.Add(new FatigueProcessUpdateSystem());
        this.esl.Add(new FatigueMoveUpdateSystem(restSources));
        this.esl.Add(new FatigueSleepingUpdateSystem(restSources));
        this.esl.Add(new FatigueSleepThristingSyncUpdateSystem());
        this.esl.Add(new FatigueToSleepUpdateSystem());
        this.esl.Add(new FollowMouseSystem());
        this.esl.Add(new MovingUpdateSystem(this.context));
        this.esl.Add(new PersonDecisionBuildAvailabilitySyncUpdateSystem());
        this.esl.Add(new PersonDecisionDrinkAvailabilitySyncUpdateSystem());
        this.esl.Add(new PersonDecisionUpdateSystem(waterSources, constructionSource, restSources));
        this.esl.Add(new PositionBindToMapSystem(this.context));
        this.esl.Add(new PositionUpdateSystem(this.context));
        this.esl.Add(new ReproductionUpdateSystem(el));
        this.esl.Add(new SelectingEntitySystem());
        this.esl.Add(new SelectPositionMouseSystem(this));
        this.esl.Add(new WalkingUpdateSystem());

        this.esl.AddExecutionOrder<FollowMouseSystem, PositionBindToMapSystem>();
        this.esl.AddExecutionOrder<PositionBindToMapSystem, PositionUpdateSystem>();

        this.render_esl = new EntitySystemList(el);
    }

    public GameContext context;
    public readonly EntityList el;
    public readonly EntitySystemList esl;
    public readonly EntitySystemList render_esl;
    public readonly EntitySystemList input_esl;

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
        e = Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0);
        e.GetComponent<PositionComponent>().Position = new Vector2(0, 0);
        this.el.Add(e);
        e = Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0);
        e.GetComponent<PositionComponent>().Position = new Vector2(size * stepX, 0);
        this.el.Add(e);
        e = Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0);
        e.GetComponent<PositionComponent>().Position = new Vector2(0, size * stepY);
        this.el.Add(e);
        e = Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0);
        e.GetComponent<PositionComponent>().Position = new Vector2(size * stepX, size * stepY);
        this.el.Add(e);

        for (var i = 1; i < size; i++)
        {
            e = Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0);
            e.GetComponent<PositionComponent>().Position = new Vector2(0, i * stepY);
            this.el.Add(e);
            e = Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0);
            e.GetComponent<PositionComponent>().Position = new Vector2(i * stepX, 0);
            this.el.Add(e);
            e = Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0);
            e.GetComponent<PositionComponent>().Position = new Vector2(size * stepX, i * stepY);
            this.el.Add(e);
            e = Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0);
            e.GetComponent<PositionComponent>().Position = new Vector2(i * stepX, size * stepY);
            this.el.Add(e);
        }
    }
}
