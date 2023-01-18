using System;
using LocomotorECS;

public class ConstructionComponent : Component
{
    public float BuildProgress { get; set; }

    public Action<Entity> ConstructionDone { get; set; }
}
