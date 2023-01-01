using System;
using BrainAI.AI.UtilityAI;
using Godot;

public interface IBuildItemActionContext
{
    float MaxHP { get; }
    float BuildHP { get; }
    float HP { get; }
    Vector2 Position { get; }

    void Build(float hp);
}

public interface IBuildActionContext
{
    IBuildItemActionContext Build { get; set; }
    float BuildSpeed { get; }
    float Delta { get; }
}

public class BuildAction<T> : IAction<T> where T : IBuildActionContext
{
    public BuildAction()
    {
    }

    public void Enter(T context)
    {
    }

    public void Execute(T context)
    {
        context.Build.Build(context.BuildSpeed * context.Delta);
        if (context.Build.MaxHP == context.Build.BuildHP)
        {
            context.Build = null;
        }
    }

    public void Exit(T context)
    {
    }
}

