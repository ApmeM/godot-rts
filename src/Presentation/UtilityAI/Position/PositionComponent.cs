using Godot;
using LocomotorECS;

public class PositionComponent : Component
{
    public Vector2 Position;
    public Vector2[] BlockingCells = new Vector2[0];
}
