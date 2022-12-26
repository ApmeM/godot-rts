using Godot;
using GodotAnalysers;

[SceneReference("Well.tscn")]
public partial class Well
{
    [Export]
    public float MaxAmount { get; set; } = 100;

    [Export]
    public float Regeneration { get; set; } = 5;

    public float CurrentAmount { get; set; } = 50;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Groups.WaterToDrink);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.label.Text = this.CurrentAmount.ToString("#");

        if (CurrentAmount < MaxAmount)
        {
            CurrentAmount += delta * Regeneration;
            if (CurrentAmount > MaxAmount)
            {
                CurrentAmount = MaxAmount;
            }
        }
    }
}
