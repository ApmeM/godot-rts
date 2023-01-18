using System.Collections.Generic;
using Godot;
using LocomotorECS;

public class MovingComponent : Component
{
    public List<Vector2> Path { get; set; } = new List<Vector2>();

    public Vector2 PathTarget { get; set; } = Vector2.Inf;

    public float MoveSpeed { get; set; } = 128;
}
