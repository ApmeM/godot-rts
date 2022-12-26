using Godot;
using GodotAnalysers;
using System;

[SceneReference("Tree.tscn")]
public partial class Tree
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }
}
