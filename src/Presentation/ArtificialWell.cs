using GodotAnalysers;
using Godot;
using LocomotorECS;

[SceneReference("ArtificialWell.tscn")]
public partial class ArtificialWell
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
        entity.GetOrCreateComponent<AvailabilityComponent>().MaxNumberOfUsers = 1;
        entity.GetOrCreateComponent<ConstructionComponent>().ConstructionDone = (e) =>
        {
            e.GetOrCreateComponent<AvailabilityComponent>().MaxNumberOfUsers = 5;
            e.GetOrCreateComponent<DrinkableComponent>().CurrentAmount = 0;
            e.GetOrCreateComponent<DrinkableRegenerationComponent>().Regeneration = 1000;
            e.GetOrCreateComponent<DrinkableRegenerationComponent>().MaxAmount = 1000;
        };
        return entity;
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

            this.label.Text = this.e.GetComponent<DrinkableComponent>().CurrentAmount.ToString("#") + "\n" +
                $"{availability.CurrentUsers.Count} / {availability.MaxNumberOfUsers}";
        }
        else
        {
            this.label.Text = (construction.BuildProgress * 100).ToString("#") + "%\n" +
                $"{availability.CurrentUsers.Count} / {availability.MaxNumberOfUsers}";
        }
    }
}
