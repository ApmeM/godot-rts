using GodotAnalysers;
using Godot;

[SceneReference("Well.tscn")]
public partial class Well
{
    public class Context : TileMapObject.Context
    {
        public float MaxAmount = 100;

        public float Regeneration = 10;

        public float CurrentAmount = 50;

        public void Tick(float delta)
        {
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

    private Context myContext => (Context)this.context;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.AddToGroup(Groups.WaterToDrink);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.myContext.Tick(delta);

        this.label.Text = this.myContext.CurrentAmount.ToString("#");
    }

    public override void InitContext(Map.Context mapContext)
    {
        this.context = this.context ?? new Context();
        base.InitContext(mapContext);
    }

}
