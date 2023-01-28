using GodotAnalysers;
using Godot;
using LocomotorECS;

[SceneReference("Well.tscn")]
public partial class Well
{
    public readonly Entity e = BuildEntity();

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
        this.GetParent<Map>().el.Add(e);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        this.GetParent<Map>().el.Remove(e);
    }

    public static Entity BuildEntity()
    {
        var entity = new Entity();
        entity.GetOrCreateComponent<PositionComponent>();
        entity.GetOrCreateComponent<AvailabilityComponent>().MaxNumberOfUsers = 1;
        entity.GetOrCreateComponent<DrinkableComponent>().CurrentAmount = 50;
        entity.GetOrCreateComponent<DrinkableRegenerationComponent>().MaxAmount = 100;
        entity.GetOrCreateComponent<DrinkableRegenerationComponent>().Regeneration = 10;
        return entity;
    }
}
