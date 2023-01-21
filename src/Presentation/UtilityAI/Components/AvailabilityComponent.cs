using System.Collections.Generic;
using LocomotorECS;

public class AvailabilityComponent : Component
{
    public int MaxNumberOfBuilders { get; set; }

    public HashSet<Entity> CurrentBuilders { get; private set; } = new HashSet<Entity>();

    public bool IsAvailable(Entity entity)
    {
        return CurrentBuilders.Contains(entity) || CurrentBuilders.Count < MaxNumberOfBuilders;
    }
}
