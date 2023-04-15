using System;
using System.Linq.Struct;
using System.Numerics;
using LocomotorECS;

public class FatigueSleepingUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> restSources;

    private readonly CommonLambdas.EntityData entityData;
    private MultiHashSetWrapper<Entity> wrapper;
    private RefLinqEnumerable<Entity, OrderBy<Entity, Where<Entity, MultiHashSetWrapperEnumerator<Entity>>, float>> query;

    public FatigueSleepingUpdateSystem(EntityLookup<int> restSources) : base(new Matcher()
        .All<FatigueSleepComponent>()
        .All<FatigueComponent>()
        .All<PositionComponent>()
        .All<PlayerComponent>())
    {
        this.restSources = restSources;
        this.entityData = new CommonLambdas.EntityData();
        this.wrapper = new MultiHashSetWrapper<Entity>();
        this.query = this.wrapper
            .Where(CommonLambdas.GetAvailabilityLambda(this.entityData))
            .OrderBy(CommonLambdas.GetEntityDistanceLambda(this.entityData));
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var fatigue = entity.GetComponent<FatigueComponent>();
        var position = entity.GetComponent<PositionComponent>();
        var player = entity.GetComponent<PlayerComponent>();

        this.entityData.Entity = entity;

        wrapper.Data = restSources[player.PlayerId].Entities;
        var closestSource = query.FirstOrDefault();
        var closestRest = closestSource?.GetComponent<PositionComponent>()?.Position ?? Vector2Ext.Inf;

        if (position.Position != closestRest || closestRest == Vector2Ext.Inf)
        {
            fatigue.CurrentFatigue -= fatigue.DefaultRest * delta;
            entity.GetComponent<PersonDecisionSleepComponent>().SelectedHouse?.GetComponent<AvailabilityComponent>().CurrentUsers.Remove(entity);
            entity.GetComponent<PersonDecisionSleepComponent>().SelectedHouse = null;
        }
        else
        {
            var rest = closestSource.GetComponent<RestComponent>();
            fatigue.CurrentFatigue -= rest.Regeneration * delta;
            closestSource?.GetComponent<AvailabilityComponent>()?.CurrentUsers.Add(entity);
        }

        if (fatigue.CurrentFatigue <= 0)
        {
            closestSource?.GetComponent<AvailabilityComponent>()?.CurrentUsers.Remove(entity);
            fatigue.CurrentFatigue = 0;
            entity.GetComponent<FatigueSleepComponent>().Disable();
            return;
        }
    }
}
