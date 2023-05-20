using Leopotam.EcsLite;

public class PersonDecisionUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<PersonComponent>()
            .Inc<PlayerComponent>()
            .Exc<FatigueSleepComponent>()
            .Exc<DeadComponent>()
            .End();

        var waterSources = world.Filter()
            .Inc<DrinkableComponent>()
            .Inc<PositionComponent>()
            .End();
        var restSources = world.Filter()
            .Inc<RestComponent>()
            .Inc<PositionComponent>()
            .End();
        var constructionSource = world.Filter()
            .Inc<ConstructionComponent>()
            .Inc<PositionComponent>()
            .End();


        var fatigueAvailabilityHolders = world.Filter()
            .Inc<PersonDecisionSleepComponent>()
            .Inc<FatigueComponent>()
            .Inc<AvailabilityHolderComponent>()
            .End();

        var drinkAvailabilityHolders = world.Filter()
            .Inc<PersonDecisionDrinkComponent>()
            .Inc<DrinkThristingComponent>()
            .Inc<AvailabilityHolderComponent>()
            .End();

        var buildAvailabilityHolders = world.Filter()
            .Inc<PersonDecisionBuildComponent>()
            .Inc<BuilderComponent>()
            .Inc<AvailabilityHolderComponent>()
            .End();

        var players = world.GetPool<PlayerComponent>();
        var prints = world.GetPool<PrintComponent>();
        var thristings = world.GetPool<DrinkThristingComponent>();
        var decisionDrinks = world.GetPool<PersonDecisionDrinkComponent>();
        var fatigues = world.GetPool<FatigueComponent>();
        var decisionSleeps = world.GetPool<PersonDecisionSleepComponent>();
        var builders = world.GetPool<BuilderComponent>();
        var decisionBuilds = world.GetPool<PersonDecisionBuildComponent>();
        var decisionWalks = world.GetPool<PersonDecisionWalkComponent>();

        foreach (var entity in filter)
        {
            ref var player = ref players.GetAdd(entity);

            var thristing = thristings.GetAdd(entity);
            if (thristing.CurrentThristing < thristing.ThristThreshold ||
                    thristing.CurrentThristing < thristing.DoneThreshold && decisionDrinks.Has(entity))
            {
                if (CommonLambdas.FindClosestAvailableSource(world, waterSources, entity, drinkAvailabilityHolders, true) > -1)
                {
                    prints.GetAdd(entity).Text = "Drink";
                    this.SetDecision<PersonDecisionDrinkComponent>(world, entity);
                    continue;
                }
            }

            var fatigue = fatigues.GetAdd(entity);
            if (fatigue.CurrentFatigue > fatigue.FatigueThreshold && CommonLambdas.FindClosestAvailableSource(world, restSources, entity, fatigueAvailabilityHolders, false) > -1)
            {
                prints.GetAdd(entity).Text = "Sleep";
                this.SetDecision<PersonDecisionSleepComponent>(world, entity);
                continue;
            }

            var builder = builders.GetAdd(entity);
            if (CommonLambdas.FindClosestAvailableSource(world, constructionSource, entity, buildAvailabilityHolders, false) > -1)
            {
                prints.GetAdd(entity).Text = "Build";
                this.SetDecision<PersonDecisionBuildComponent>(world, entity);
                continue;
            }

            prints.GetAdd(entity).Text = "Walk";
            this.SetDecision<PersonDecisionWalkComponent>(world, entity);
        }
    }

    private void SetDecision<T>(EcsWorld world, int entity) where T : struct
    {
        CheckDecision<T, PersonDecisionDrinkComponent>(world, entity);
        CheckDecision<T, PersonDecisionSleepComponent>(world, entity);
        CheckDecision<T, PersonDecisionBuildComponent>(world, entity);
        CheckDecision<T, PersonDecisionWalkComponent>(world, entity);
    }


    private void CheckDecision<T1, T2>(EcsWorld world, int entity) where T2 : struct where T1 : struct
    {
        if (typeof(T1) != typeof(T2))
        {
            world.GetPool<T2>().Del(entity);
        }
        else
        {
            world.GetPool<T2>().GetAdd(entity);
        }
    }
}
