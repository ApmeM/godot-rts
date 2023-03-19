using System;
using System.Numerics;
using HonkPerf.NET.RefLinq;
using HonkPerf.NET.RefLinq.Enumerators;
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
        this.query = this.wrapper.ToRefLinq()
            .Where(CommonLambdas.GetAvailabilityLambda(this.entityData))
            .OrderBy(CommonLambdas.GetEntityDistanceLambda(this.entityData));
    }

    protected override void DoAction(float delta)
    {
        if (!restSources.ToRefLinq().Where(a => a.Value.Entities.Count > 0).Any())
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
        
        wrapper.Set = restSources[player.PlayerId].Entities;
        var closestSource = query.FirstOrDefault();

        var closestRest = closestSource?.GetComponent<PositionComponent>()?.Position ?? Vector2Ext.Inf;

        if (position.Position == closestRest)
        {
            entity.GetOrCreateComponent<FatigueSleepComponent>().Enable();
            return;
        }

        entity.GetComponent<MovingComponent>().PathTarget = closestRest;
    }
}
