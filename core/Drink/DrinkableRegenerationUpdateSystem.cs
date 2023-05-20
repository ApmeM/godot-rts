using Leopotam.EcsLite;

public class DrinkableRegenerationUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var delta = systems.GetShared<World.SharedData>().delta;
        
        var world = systems.GetWorld();

        var filter = world.Filter()
            .Inc<DrinkableRegenerationComponent>()
            .Inc<DrinkableComponent>()
            .End();

        var drinkables = world.GetPool<DrinkableComponent>();
        var regenerations = world.GetPool<DrinkableRegenerationComponent>();

        foreach (var entity in filter)
        {
            ref var drinkable = ref drinkables.GetAdd(entity);
            var regeneration = regenerations.GetAdd(entity);

            if (drinkable.CurrentAmount >= regeneration.MaxAmount)
            {
                continue;
            }

            drinkable.CurrentAmount += delta * regeneration.Regeneration;
            if (drinkable.CurrentAmount > regeneration.MaxAmount)
            {
                drinkable.CurrentAmount = regeneration.MaxAmount;
            }
        }
    }
}