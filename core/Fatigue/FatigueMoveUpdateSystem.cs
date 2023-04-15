using System;
using System.Linq.Struct;
using System.Numerics;
using LocomotorECS;

public class FatigueMoveUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> restSources;

    private readonly CommonLambdas.EntityData entityData;
    private MultiHashSetWrapper<Entity> wrapper;
    private RefLinqEnumerable<Entity, OrderBy<Entity, Where<Entity, MultiHashSetWrapperEnumerator<Entity>>, float>> query;

    public FatigueMoveUpdateSystem(EntityLookup<int> restSources) : base(new Matcher()
        .All<PersonDecisionSleepComponent>()
        .All<PositionComponent>()
        .All<MovingComponent>()
        .All<PlayerComponent>()
        .Exclude<FatigueSleepComponent>())
    {
        this.restSources = restSources;
        this.entityData = new CommonLambdas.EntityData();
        this.wrapper = new MultiHashSetWrapper<Entity>();
        this.query = this.wrapper
            .Where(CommonLambdas.GetAvailabilityLambda(this.entityData))
            .OrderBy(CommonLambdas.GetEntityDistanceLambda(this.entityData));
    }

    protected override void DoAction(float delta)
    {
        if (!restSources.Where(a => a.Value.Entities.Count > 0).Any())
        {
            return;
        }

        base.DoAction(delta);
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var position = entity.GetComponent<PositionComponent>();
        var player = entity.GetComponent<PlayerComponent>();

        this.entityData.Entity = entity;
        
        wrapper.Data = restSources[player.PlayerId].Entities;
        var closestSource = query.FirstOrDefault();

        var closestRest = closestSource?.GetComponent<PositionComponent>()?.Position ?? Vector2Ext.Inf;

        if (position.Position == closestRest)
        {
            entity.GetComponent<FatigueSleepComponent>().Enable();
            return;
        }

        entity.GetComponent<MovingComponent>().PathTarget = closestRest;
    }
}
