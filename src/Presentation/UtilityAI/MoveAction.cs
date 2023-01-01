using System.Collections.Generic;
using BrainAI.AI.UtilityAI;
using Godot;

public interface IMoveActionContext
{
    Vector2 Position { get; }
    Map.Context MapContext { get; }
    List<Vector2> Path { get; }
    float MoveSpeed { get; }
    float Delta { get; }
    void Move(Vector2 delta);
}

public class MoveAction<T> : IAction<T> where T : IMoveActionContext
{
    public MoveAction()
    {
    }

    public void Enter(T context)
    {
    }

    public void Execute(T context)
    {
        var current = context.Position;
        var destination = context.MapContext.MapToWorld(context.Path[0]);
        var path = destination - current;
        var motion = path.Normalized() * context.MoveSpeed * context.Delta;
        if (path.LengthSquared() > motion.LengthSquared())
        {
            context.Move(motion);
        }
        else
        {
            context.Move(destination - context.Position);
            context.Path.RemoveAt(0);
        }
    }

    public void Exit(T context)
    {
    }
}

