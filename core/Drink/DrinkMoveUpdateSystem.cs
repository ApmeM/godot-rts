using System;
using System.Linq.Struct;
using System.Numerics;
using LocomotorECS;

public class DrinkMoveUpdateSystem : MatcherEntitySystem
{
    private readonly EntityLookup<int> waterSources;

    private readonly CommonLambdas.EntityData entityData;
    private MultiHashSetWrapper<Entity> wrapper;
    private RefLinqEnumerable<Entity, OrderBy<Entity, Where<Entity, MultiHashSetWrapperEnumerator<Entity>>, float>> query;

    public DrinkMoveUpdateSystem(EntityLookup<int> drinkSource) : base(new Matcher()
        .All<PersonDecisionDrinkComponent>()
        .All<MovingComponent>()
        .All<PositionComponent>()
        .Exclude<FatigueSleepComponent>())
    {
        this.waterSources = drinkSource;

        this.entityData = new CommonLambdas.EntityData();
        this.wrapper = new MultiHashSetWrapper<Entity>();
        this.query = this.wrapper
            .Where(CommonLambdas.GetAvailabilityLambda(this.entityData))
            .OrderBy(CommonLambdas.GetEntityDistanceLambda(this.entityData));
    }

    protected override void DoAction(float delta)
    {
        if (!waterSources.Where(a => a.Value.Entities.Count > 0).Any())
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

        wrapper.Data = waterSources[0].Entities;
        var closestNeutralSource = query.FirstOrDefault();
        wrapper.Data = waterSources[player.PlayerId].Entities;
        var closestSelfSource = query.FirstOrDefault();

        var closestSource = closestNeutralSource == null
            ? closestSelfSource
            : closestSelfSource == null
                ? closestNeutralSource
                : (closestNeutralSource.GetComponent<PositionComponent>().Position -
                   entity.GetComponent<PositionComponent>().Position).LengthSquared() >
                  (closestSelfSource.GetComponent<PositionComponent>().Position -
                   entity.GetComponent<PositionComponent>().Position).LengthSquared()
                    ? closestSelfSource
                    : closestNeutralSource;

        var closestWater = closestSource?.GetComponent<PositionComponent>()?.Position ?? Vector2Ext.Inf;
        if (position.Position == closestWater)
        {
            return;
        }

        entity.GetComponent<MovingComponent>().PathTarget = closestWater;
    }
}
