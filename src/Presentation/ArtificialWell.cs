using GodotAnalysers;
using Godot;
using LocomotorECS;

[SceneReference("ArtificialWell.tscn")]
public partial class ArtificialWell
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
        e.GetOrCreateComponent<ConstructionComponent>().MaxNumberOfBuilders = 1;
        e.GetOrCreateComponent<ConstructionComponent>().ConstructionDone = (e) =>
        {
            e.GetOrCreateComponent<DrinkableComponent>().CurrentAmount = 0;
            e.GetOrCreateComponent<DrinkableRegenerationComponent>().Regeneration = 1000;
            e.GetOrCreateComponent<DrinkableRegenerationComponent>().MaxAmount = 1000;
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

            this.label.Text = this.e.GetComponent<DrinkableComponent>().CurrentAmount.ToString("#");
        }
        else
        {
            this.label.Text = (construction.BuildProgress * 100).ToString("#") + "%\n" +
                $"{construction.CurrentBuilders.Count} / {construction.MaxNumberOfBuilders}";
        }
    }
}
