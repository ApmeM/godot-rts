using System.Collections.Generic;
using Godot;
using LocomotorECS;

public class MovingComponent : Component
{
    public List<Vector2> Path = new List<Vector2>();

    public Vector2 PathTarget = Vector2.Inf;

    public float MoveSpeed;
}
