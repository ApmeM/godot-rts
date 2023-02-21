using System.Collections.Generic;
using LocomotorECS;

public class AvailabilityComponent : Component
{
    public int MaxNumberOfUsers;

    public readonly HashSet<Entity> CurrentUsers = new HashSet<Entity>();

    public bool IsAvailable(Entity entity)
    {
        return CurrentUsers.Contains(entity) || CurrentUsers.Count < MaxNumberOfUsers;
    }
}
