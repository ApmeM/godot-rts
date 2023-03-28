using System;
using System.Collections.Generic;
using HonkPerf.NET.RefLinq;
using HonkPerf.NET.RefLinq.Enumerators;
using LocomotorECS;

public class PersonDecisionUpdateSystem : MatcherEntitySystem
{
    private readonly EntityLookup<int> waterSources;
    private readonly EntityLookup<int> constructionSource;
    private readonly EntityLookup<int> restSources;

    private readonly CommonLambdas.EntityData entityData;
    private MultiHashSetWrapper<Entity> wrapper;
    private readonly RefLinqEnumerable<Entity, Where<Entity, MultiHashSetWrapperEnumerator<Entity>>> query;

    public PersonDecisionUpdateSystem(
        EntityLookup<int> waterSources,
        EntityLookup<int> constructionSource,
        EntityLookup<int> restSources) : base(new Matcher()
            .All<PersonComponent>()
            .All<PlayerComponent>()
            .Exclude<FatigueSleepComponent>())
    {
        this.waterSources = waterSources;
        this.constructionSource = constructionSource;
        this.restSources = restSources;

        this.entityData = new CommonLambdas.EntityData();
        this.wrapper = new MultiHashSetWrapper<Entity>();
        this.query = this.wrapper.ToRefLinq().Where(CommonLambdas.GetAvailabilityLambda(this.entityData));
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var player = entity.GetComponent<PlayerComponent>();

        this.entityData.Entity = entity;

        var thristing = entity.GetComponent<DrinkThristingComponent>();
        if (thristing != null && (
                thristing.CurrentThristing < thristing.ThristThreshold ||
                thristing.CurrentThristing < thristing.MaxThristLevel && entity.GetComponent<PersonDecisionDrinkComponent>().Enabled))
        {
            wrapper.Set = waterSources[0].Entities;
            var result = query.Any();
            wrapper.Set = waterSources[player.PlayerId].Entities;
            result = result || query.Any();
            if (result)
            {
                entity.GetComponent<PrintComponent>().Text = "Drink";
                this.SetDecision<PersonDecisionDrinkComponent>(entity);
                return;
            }
        }

        wrapper.Set = restSources[player.PlayerId].Entities;
        var fatigue = entity.GetComponent<FatigueComponent>();
        if (fatigue != null && fatigue.CurrentFatigue > fatigue.FatigueThreshold && query.Any())
        {
            entity.GetComponent<PrintComponent>().Text = "Sleep";
            this.SetDecision<PersonDecisionSleepComponent>(entity);
            return;
        }

        wrapper.Set = constructionSource[player.PlayerId].Entities;
        var builder = entity.GetComponent<BuilderComponent>();
        if (builder != null && query.Any())
        {
            entity.GetComponent<PrintComponent>().Text = "Build";
            this.SetDecision<PersonDecisionBuildComponent>(entity);
            return;
        }

        this.SetDecision<PersonDecisionWalkComponent>(entity);
        entity.GetComponent<PrintComponent>().Text = "Walk";
    }

    private void SetDecision<T>(Entity entity)
    {
        CheckDecision<T, PersonDecisionDrinkComponent>(entity);
        CheckDecision<T, PersonDecisionSleepComponent>(entity);
        CheckDecision<T, PersonDecisionBuildComponent>(entity);
        CheckDecision<T, PersonDecisionWalkComponent>(entity);
    }


    private void CheckDecision<T1, T2>(Entity entity) where T2 : Component
    {
        if (typeof(T1) != typeof(T2))
        {
            entity.GetComponent<T2>().Disable();
        }
        else
        {
            entity.GetComponent<T2>().Enable();
        }
    }
}
