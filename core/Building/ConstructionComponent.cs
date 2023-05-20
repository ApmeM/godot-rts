using System;

public struct ConstructionComponent
{
    public float BuildProgress;
    
    public float MaxBuildProgress;

    public Action<int> ConstructionDone;
}
