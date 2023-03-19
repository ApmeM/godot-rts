using BrainAI.Pathfinding;
using System;
using System.Collections.Generic;
using System.Numerics;
using LocomotorECS;

public static class CommonLambdas
{
    public class EntityData{
        public Entity Entity;
    }

    public static Func<Entity, float> GetEntityDistanceLambda(EntityData data)
    {
        return a => (a.GetComponent<PositionComponent>().Position - data.Entity.GetComponent<PositionComponent>().Position).LengthSquared();
    } 

    public static Func<Entity, bool> GetAvailabilityLambda(EntityData data)
    {
        return a => a.GetComponent<AvailabilityComponent>()?.IsAvailable(data.Entity) ?? true;
    } 
}
