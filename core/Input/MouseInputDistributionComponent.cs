using System;
using System.Numerics;
using LocomotorECS;
public class MouseInputDistributionComponent : Component
{
    public Vector2 MousePosition;
    public int MouseButtons;
    public int JustReleasedButtins;
    public int JustPressedButtins;
}
