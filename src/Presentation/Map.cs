using GodotAnalysers;
using Godot;
using System;

[SceneReference("Map.tscn")]
public partial class Map
{
    public event Action<InputEvent> UnhandledInput;

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        this.UnhandledInput?.Invoke(@event);
    }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }
}
