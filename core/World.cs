using System;
using System.Numerics;
using LocomotorECS;

public class World
{
    public World(Func<Vector2, Vector2> worldToMap, Func<Vector2, Vector2> mapToWorld)
    {
        this.context = new GameContext(worldToMap, mapToWorld);
        this.el = new EntityList();

        var inputEntity = this.el.Add(new Entity());
        inputEntity.AddComponent<MouseInputDistributionComponent>();

        var notifyEntity = this.el.Add(new Entity());
        notifyEntity.AddComponent<NotificationComponent>();

        this.el.CommitChanges();

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
        this.input_esl.Add(new MouseInputDistributeSystem(inputEntity));

        this.esl = new EntitySystemList(el);
        this.esl.Add(new BuildMoveUpdateSystem(constructionSource));
        this.esl.Add(new BuildProcessUpdateSystem(constructionSource));
        this.esl.Add(new CleanupNotificationsUpdateSystem());
        this.esl.Add(new DrinkableRegenerationUpdateSystem());
        this.esl.Add(new DrinkMoveUpdateSystem(waterSources));
        this.esl.Add(new DrinkProcessUpdateSystem(waterSources));
        this.esl.Add(new DrinkThristingDeathUpdateSystem(notifyEntity));
        this.esl.Add(new DrinkThristingUpdateSystem());
        this.esl.Add(new FatigueProcessUpdateSystem());
        this.esl.Add(new FatigueMoveUpdateSystem(restSources));
        this.esl.Add(new FatigueSleepingUpdateSystem(restSources));
        this.esl.Add(new FatigueSleepThristingSyncUpdateSystem());
        this.esl.Add(new FatigueToSleepUpdateSystem(notifyEntity));
        this.esl.Add(new FollowMouseSystem());
        this.esl.Add(new MovingUpdateSystem(this.context));
        this.esl.Add(new PersonDecisionBuildAvailabilitySyncUpdateSystem());
        this.esl.Add(new PersonDecisionDrinkAvailabilitySyncUpdateSystem());
        this.esl.Add(new PersonDecisionUpdateSystem(waterSources, constructionSource, restSources));
        this.esl.Add(new PositionBindToMapSystem(this.context));
        this.esl.Add(new PositionUpdateSystem(this.context));
        this.esl.Add(new RemoveDeadSystem(this));
        this.esl.Add(new ReproductionUpdateSystem(el));
        this.esl.Add(new SelectingEntitySystem());
        this.esl.Add(new SelectPositionMouseSystem(this));
        this.esl.Add(new WalkingUpdateSystem());

        this.esl.AddExecutionOrder<CleanupNotificationsUpdateSystem, DrinkThristingDeathUpdateSystem>();
        this.esl.AddExecutionOrder<CleanupNotificationsUpdateSystem, FatigueToSleepUpdateSystem>();

        this.esl.AddExecutionOrder<DrinkThristingDeathUpdateSystem, RemoveDeadSystem>();

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

    public void BuildForTest(float stepX, float stepY)
    {
        const int myPlayerId = 1;
        const int fenceSize = 60;

        el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Well, myPlayerId)).GetComponent<PositionComponent>().Position = new Vector2(2 * stepX, 2 * stepY);

        for (var i = 0; i < 4; i++)
        {
            el.Add(Entities.Build(EntityTypeComponent.EntityTypes.House, myPlayerId)).GetComponent<PositionComponent>().Position = new Vector2(5 * stepX, (2 + 5 * i) * stepY);
            el.Add(Entities.Build(EntityTypeComponent.EntityTypes.ArtificialWell, myPlayerId)).GetComponent<PositionComponent>().Position = new Vector2(10 * stepX, (2 + 5 * i) * stepY);
        }

        for (var i = 0; i < 6; i++)
            for (var j = 0; j < 6; j++)
            {
                el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Person, myPlayerId)).GetComponent<PositionComponent>().Position = new Vector2((15 + i) * stepX, (15 + j) * stepY);
            }

        el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0)).GetComponent<PositionComponent>().Position = new Vector2(0, 0);
        el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0)).GetComponent<PositionComponent>().Position = new Vector2(fenceSize * stepX, 0);
        el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0)).GetComponent<PositionComponent>().Position = new Vector2(0, fenceSize * stepY);
        el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0)).GetComponent<PositionComponent>().Position = new Vector2(fenceSize * stepX, fenceSize * stepY);

        for (var i = 1; i < fenceSize; i++)
        {
            el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0)).GetComponent<PositionComponent>().Position = new Vector2(0, i * stepY);
            el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0)).GetComponent<PositionComponent>().Position = new Vector2(i * stepX, 0);
            el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0)).GetComponent<PositionComponent>().Position = new Vector2(fenceSize * stepX, i * stepY);
            el.Add(Entities.Build(EntityTypeComponent.EntityTypes.Tree, 0)).GetComponent<PositionComponent>().Position = new Vector2(i * stepX, fenceSize * stepY);
        }
    }
}
