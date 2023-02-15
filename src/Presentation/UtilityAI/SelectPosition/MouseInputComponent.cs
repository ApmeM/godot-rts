using Godot;
using LocomotorECS;
public class MouseInputComponent : Component
{
    public Vector2 MousePosition;
    public int MouseButtons;
    public int JustReleasedButtins;
    public int JustPressedButtins;
}
