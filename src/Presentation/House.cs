using GodotAnalysers;
using Godot;
using LocomotorECS;

[SceneReference("House.tscn")]
public partial class House
{
    public readonly Entity e = Entities.BuildHouse();

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

    public override void _Process(float delta)
    {
        base._Process(delta);

        var construction = this.e.GetComponent<ConstructionComponent>();
        var availability = this.e.GetComponent<AvailabilityComponent>();

        if (construction == null)
        {
            this.sprite.Hide();
            this.sprite1.Show();
            this.label.Text = $"{availability.CurrentUsers.Count} / {availability.MaxNumberOfUsers}";
        }
        else
        {
            this.label.Text = (construction.BuildProgress * 100).ToString("#") + "%\n" +
                $"{availability.CurrentUsers.Count} / {availability.MaxNumberOfUsers}";
        }
    }
}
