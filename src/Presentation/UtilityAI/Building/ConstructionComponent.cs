using System;
using System.Collections.Generic;
using LocomotorECS;

public class ConstructionComponent : Component
{
    public float BuildProgress;

    public Action<Entity> ConstructionDone;
}
