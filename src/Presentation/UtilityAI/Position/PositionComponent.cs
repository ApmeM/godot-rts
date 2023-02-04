using Godot;
using LocomotorECS;

public class PositionComponent : Component
{
    public Vector2 Position { get; set; }
    public Vector2[] BlockingCells { get; set; } = new Vector2[0];
}
