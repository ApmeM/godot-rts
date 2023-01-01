using Godot;

public class MoveUnitAction : IUnitAction
{
    private const int DEFAULT_MOVE_SPEED = 160;

    private readonly TileMapObject.Context unit;
    private readonly Vector2 destination;

    public MoveUnitAction(TileMapObject.Context unit, Vector2 destination, float moveSpeed = DEFAULT_MOVE_SPEED)
    {
        this.unit = unit;
        this.destination = destination;
    }

    public bool Process(float delta)
    {
        var current = this.unit.Position;
        var path = destination - current;
        var motion = path.Normalized() * DEFAULT_MOVE_SPEED * delta;
        if (path.LengthSquared() > motion.LengthSquared())
        {
            this.unit.Position += motion;
        }
        else
        {
            this.unit.Position = this.destination;
            return true;
        }

        return false;
    }
}