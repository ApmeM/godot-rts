using System;
using System.Collections.Generic;
using System.Linq.Struct;
using System.Numerics;
using LocomotorECS;

public class BuildProcessUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> constructionSource;
    private readonly CommonLambdas.EntityData entityData;
    private MultiHashSetWrapper<Entity> wrapper;
    private RefLinqEnumerable<Entity, OrderBy<Entity, Where<Entity, MultiHashSetWrapperEnumerator<Entity>>, float>> query;
    
    public BuildProcessUpdateSystem(EntityLookup<int> constructionSource) : base(new Matcher()
        .All<PersonDecisionBuildComponent>()
        .All<BuilderComponent>()
        .All<PositionComponent>()
        .All<PlayerComponent>()
        .Exclude<FatigueSleepComponent>())
    {
        this.constructionSource = constructionSource;
        this.entityData = new CommonLambdas.EntityData();
        this.wrapper = new MultiHashSetWrapper<Entity>();
        this.query = this.wrapper
            .Where(CommonLambdas.GetAvailabilityLambda(this.entityData))
            .OrderBy(CommonLambdas.GetEntityDistanceLambda(this.entityData));
    }

    protected override void DoAction(float delta)
    {
        if (!constructionSource.Where(a => a.Value.Entities.Count > 0).Any())
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
        
        wrapper.Data = constructionSource[player.PlayerId].Entities;
        var closestSource = query.FirstOrDefault();

        var closestConstruction = closestSource?.GetComponent<PositionComponent>()?.Position ?? Vector2Ext.Inf;

        if (position.Position != closestConstruction)
        {
            entity.GetComponent<PersonDecisionBuildComponent>().SelectedConstruction?.GetComponent<AvailabilityComponent>().CurrentUsers.Remove(entity);
            entity.GetComponent<PersonDecisionBuildComponent>().SelectedConstruction = null;
            return;
        }

        var construction = closestSource.GetComponent<ConstructionComponent>();
        if (construction.BuildProgress >= 1)
        {
            construction.ConstructionDone?.Invoke(closestSource);
            construction.ConstructionDone = null;
            closestSource.RemoveComponent<ConstructionComponent>();
            return;
        }

        closestSource.GetComponent<AvailabilityComponent>()?.CurrentUsers.Add(entity);
        entity.GetComponent<PersonDecisionBuildComponent>().SelectedConstruction = closestSource;

        var builder = entity.GetComponent<BuilderComponent>();

        var hpToPct = 0.01f;
        var hp = closestSource.GetComponent<HPComponent>();
        if (hp != null)
        {
            hpToPct = 1f / hp.MaxHP;
        }
        var buildProgress = builder.BuildSpeed * hpToPct * delta;
        construction.BuildProgress += buildProgress;
        if (hp != null)
        {
            hp.HP += builder.BuildSpeed;
            if (hp.HP > hp.MaxHP)
            {
                hp.HP = hp.MaxHP;
            }
        }
    }
}
