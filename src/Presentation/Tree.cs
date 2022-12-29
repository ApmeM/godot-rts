using Godot;
using GodotAnalysers;
using System;

[SceneReference("Tree.tscn")]
public partial class Tree
{
    public override void _Ready()
    {
        this.BlockingCells = new Vector2[] { Vector2.Zero };

        base._Ready();
        this.FillMembers();
    }
}
