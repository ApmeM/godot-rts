using GodotAnalysers;
using Godot;
using LocomotorECS;

[SceneReference("Well.tscn")]
public partial class Well
{
    public readonly Entity e = new Entity();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

         this.label.Text = this.e.GetComponent<DrinkableComponent>().CurrentAmount.ToString("#");
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        
        e.GetOrCreateComponent<Node2DComponent>().Node = this;
        e.GetOrCreateComponent<PositionComponent>().Position = this.Position;
        e.GetOrCreateComponent<DrinkableComponent>().CurrentAmount = 50;
        e.GetOrCreateComponent<DrinkRegenerationComponent>().MaxAmount = 100;
        e.GetOrCreateComponent<DrinkRegenerationComponent>().Regeneration = 10;
        this.GetParent<Map>().el.Add(e);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        this.GetParent<Map>().el.Remove(e);
    }
}
