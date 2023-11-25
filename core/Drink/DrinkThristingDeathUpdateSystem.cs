using Leopotam.EcsLite;

public class DrinkThristingDeathUpdateSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var thristingEntities = world.Filter()
            .Inc<DrinkThristingComponent>()
            .Exc<DeadComponent>()
            .End();

        var thristings = world.GetPool<DrinkThristingComponent>();
        var deads = world.GetPool<DeadComponent>();

        var isDead = false;

        foreach (var thristingEntity in thristingEntities)
        {
            var thristing = thristings.GetAdd(thristingEntity);

            if (thristing.CurrentThristing >= 0)
            {
                continue;
            }
            isDead = true;
            deads.Add(thristingEntity);
        }

        if(isDead){
            NotificationUtils.Notify(systems, Notifications.ThristingDead);
        }
    }
}
