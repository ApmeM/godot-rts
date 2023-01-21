using GodotAnalysers;
using Godot;
using LocomotorECS;

[SceneReference("House.tscn")]
public partial class House
{
    public readonly Entity e = new Entity();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        e.GetOrCreateComponent<Node2DComponent>().Node = this;
        e.GetOrCreateComponent<PositionComponent>().Position = this.Position;
        e.GetOrCreateComponent<PositionComponent>().BlockingCells = new Vector2[]{
            Vector2.Up,
            Vector2.Down,
            Vector2.Left,
            Vector2.Left + Vector2.Up,
            Vector2.Left + Vector2.Down,
            Vector2.Right + Vector2.Up,
            Vector2.Right + Vector2.Down,
        };
        e.GetOrCreateComponent<HPComponent>().MaxHP = 100;
        e.GetOrCreateComponent<ConstructionComponent>().ConstructionDone = (e) =>
        {
            e.GetOrCreateComponent<RestComponent>().Regeneration = 20;
        };
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

        if (construction == null)
        {
            this.sprite.Hide();
            this.sprite1.Show();
        }
        else
        {
            this.label.Text = (construction.BuildProgress * 100).ToString("#") + "%";
        }
    }
}
