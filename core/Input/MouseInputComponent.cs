using System;
using System.Numerics;

public struct MouseInputComponent
{
    public enum ButtonList
    {
        Left = 1,
        MaskLeft = 1,
        Right = 2,
        MaskRight = 2,
        Middle = 3,
        WheelUp = 4,
        MaskMiddle = 4,
        WheelDown = 5,
        WheelLeft = 6,
        WheelRight = 7,
        Xbutton1 = 8,
        Xbutton2 = 9,
        MaskXbutton1 = 128,
        MaskXbutton2 = 256
    }

    public Vector2 MousePosition;
    public int MouseButtons;
    public int JustReleasedButtins;
    public int JustPressedButtins;
}
