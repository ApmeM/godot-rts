using GodotAnalysers;
using Godot;
using LocomotorECS;

[SceneReference("Well.tscn")]
public partial class Well : EntityTypeNode2DRenderSystem.IEntityNode2D
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
        
        this.label.Text = this.e.GetComponent<DrinkableComponent>().CurrentAmount.ToString("#");
    }
}
