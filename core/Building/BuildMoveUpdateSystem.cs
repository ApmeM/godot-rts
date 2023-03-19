using System;
using System.Collections.Generic;
using System.Numerics;
using HonkPerf.NET.RefLinq;
using HonkPerf.NET.RefLinq.Enumerators;
using LocomotorECS;

public class BuildMoveUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> constructionSource;

    private readonly CommonLambdas.EntityData entityData;
    private MultiHashSetWrapper<Entity> wrapper;
    private RefLinqEnumerable<Entity, OrderBy<Entity, Where<Entity, MultiHashSetWrapperEnumerator<Entity>>, float>> query;

    public BuildMoveUpdateSystem(EntityLookup<int> constructionSource) : base(new Matcher()
        .All<PersonDecisionBuildComponent>()
        .All<MovingComponent>()
        .All<PositionComponent>()
        .All<PlayerComponent>()
        .Exclude<FatigueSleepComponent>())
    {
        this.constructionSource = constructionSource;
        this.entityData = new CommonLambdas.EntityData();
        this.wrapper = new MultiHashSetWrapper<Entity>();
        this.query = this.wrapper.ToRefLinq()
            .Where(CommonLambdas.GetAvailabilityLambda(this.entityData))
            .OrderBy(CommonLambdas.GetEntityDistanceLambda(this.entityData));
    }

    protected override void DoAction(float delta)
    {
        if (!constructionSource.ToRefLinq().Where(a => a.Value.Entities.Count > 0).Any())
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

        wrapper.Set = constructionSource[player.PlayerId].Entities;
        var closestSource = query.FirstOrDefault();

        var closestConstruction = closestSource?.GetComponent<PositionComponent>()?.Position ?? Vector2Ext.Inf;
        if (position.Position == closestConstruction)
        {
            return;
        }

        entity.GetComponent<MovingComponent>().PathTarget = closestConstruction;
    }
}
