using System;
using System.Collections.Generic;
using System.Numerics;
using HonkPerf.NET.RefLinq;
using HonkPerf.NET.RefLinq.Enumerators;
using LocomotorECS;

public class DrinkProcessUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> waterSources;

    private readonly CommonLambdas.EntityData entityData;
    private MultiHashSetWrapper<Entity> wrapper;
    private RefLinqEnumerable<Entity, OrderBy<Entity, Where<Entity, MultiHashSetWrapperEnumerator<Entity>>, float>> query;

    public DrinkProcessUpdateSystem(EntityLookup<int> drinkSource) : base(new Matcher()
        .All<PersonDecisionDrinkComponent>()
        .All<DrinkThristingComponent>()
        .All<PositionComponent>()
        .Exclude<FatigueSleepComponent>())
    {
        this.waterSources = drinkSource;
        this.entityData = new CommonLambdas.EntityData();
        this.wrapper = new MultiHashSetWrapper<Entity>();
        this.query = this.wrapper.ToRefLinq()
            .Where(CommonLambdas.GetAvailabilityLambda(this.entityData))
            .OrderBy(CommonLambdas.GetEntityDistanceLambda(this.entityData));
    }

    protected override void DoAction(float delta)
    {
        if (!waterSources.ToRefLinq().Where(a => a.Value.Entities.Count > 0).Any())
        {
            return;
        }

        base.DoAction(delta);
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var thristing = entity.GetComponent<DrinkThristingComponent>();
        var position = entity.GetComponent<PositionComponent>();
        var player = entity.GetComponent<PlayerComponent>();

        this.entityData.Entity = entity;
        
        wrapper.Set = waterSources[0].Entities;
        var closestNeutralSource = query.FirstOrDefault();
        wrapper.Set = waterSources[player.PlayerId].Entities;
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
        if (position.Position != closestWater)
        {
            entity.GetComponent<PersonDecisionDrinkComponent>().SelectedSource?.GetComponent<AvailabilityComponent>().CurrentUsers.Remove(entity);
            entity.GetComponent<PersonDecisionDrinkComponent>().SelectedSource = null;
            return;
        }

        closestSource.GetComponent<AvailabilityComponent>()?.CurrentUsers.Add(entity);
        entity.GetComponent<PersonDecisionDrinkComponent>().SelectedSource = closestSource;

        var drinkable = closestSource.GetComponent<DrinkableComponent>();
        var toDrink = Math.Min(thristing.DrinkSpeed * delta, drinkable.CurrentAmount);
        drinkable.CurrentAmount -= toDrink;
        thristing.CurrentThristing += toDrink;
    }
}
