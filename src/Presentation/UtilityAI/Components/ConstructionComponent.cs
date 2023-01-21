using System;
using System.Collections.Generic;
using LocomotorECS;

public class ConstructionComponent : Component
{
    public int MaxNumberOfBuilders { get; set; }

    public HashSet<Entity> CurrentBuilders { get; private set; } = new HashSet<Entity>();

    public float BuildProgress { get; set; }

    public Action<Entity> ConstructionDone { get; set; }
}
