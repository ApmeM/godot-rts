using System.Collections.Generic;
using LocomotorECS;

public class AvailabilityComponent : Component
{
    public int MaxNumberOfUsers { get; set; }

    public HashSet<Entity> CurrentUsers { get; private set; } = new HashSet<Entity>();

    public bool IsAvailable(Entity entity)
    {
        return CurrentUsers.Contains(entity) || CurrentUsers.Count < MaxNumberOfUsers;
    }
}
