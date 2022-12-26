using System;
using Godot;

public class CallbackUnitAction : IUnitAction
{
    private readonly Action callback;

    public CallbackUnitAction(Action callback)
    {
        this.callback = callback;
    }

    public bool Process(float delta)
    {
        callback?.Invoke();
        return true;
    }
}