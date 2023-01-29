using Godot;
using GodotAnalysers;
using LocomotorECS;

[SceneReference("Tree.tscn")]
public partial class Tree
{
    public readonly Entity e = Entities.BuildTree();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        e.GetOrCreateComponent<Node2DComponent>().Node = this;
        this.GetParent<Map>().el.Add(e);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        this.GetParent<Map>().el.Remove(e);
    }
}
