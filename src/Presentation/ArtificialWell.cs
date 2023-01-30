using GodotAnalysers;
using Godot;
using LocomotorECS;

[SceneReference("ArtificialWell.tscn")]
public partial class ArtificialWell : EntityTypeNode2DRenderSystem.IEntityNode2D
{
    public Entity e { get; set; }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
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
