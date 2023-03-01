using System.Collections.Generic;
using System.Numerics;
using LocomotorECS;

public class MovingComponent : Component
{
    public List<Vector2> Path = new List<Vector2>();

    public Vector2 PathTarget = Vector2Ext.Inf;

    public float MoveSpeed;
}
