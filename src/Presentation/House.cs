using GodotAnalysers;
using Godot;
using LocomotorECS;

[SceneReference("House.tscn")]
public partial class House
{
    public readonly Entity e = BuildEntity();

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
    public static Entity BuildEntity()
    {
        var entity = new Entity();
        entity.GetOrCreateComponent<PositionComponent>().BlockingCells = new Vector2[]{
            Vector2.Up,
            Vector2.Down,
            Vector2.Left,
            Vector2.Left + Vector2.Up,
            Vector2.Left + Vector2.Down,
            Vector2.Right + Vector2.Up,
            Vector2.Right + Vector2.Down,
        };
        entity.GetOrCreateComponent<HPComponent>().MaxHP = 100;
        entity.GetOrCreateComponent<AvailabilityComponent>().MaxNumberOfUsers = 2;
        entity.GetOrCreateComponent<ConstructionComponent>().ConstructionDone = (e) =>
        {
            e.GetOrCreateComponent<AvailabilityComponent>().MaxNumberOfUsers = 10;
            e.GetOrCreateComponent<RestComponent>().Regeneration = 20;
        };
        return entity;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        var construction = this.e.GetComponent<ConstructionComponent>();

        if (construction == null)
        {
            this.sprite.Hide();
            this.sprite1.Show();
            this.label.Text = "";
        }
        else
        {
            var availability = this.e.GetComponent<AvailabilityComponent>();
            this.label.Text = (construction.BuildProgress * 100).ToString("#") + "%\n" +
                $"{availability.CurrentUsers.Count} / {availability.MaxNumberOfUsers}";
        }
    }
}
