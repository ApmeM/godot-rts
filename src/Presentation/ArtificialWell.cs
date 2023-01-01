using GodotAnalysers;
using Godot;

[SceneReference("ArtificialWell.tscn")]
public partial class ArtificialWell
{
    public class Context : Construction.Context, IDrinkFromActionContext
    {
        public Context(float maxHP) : base(maxHP)
        {
        }

        public float CurrentAmount { get; set; }

        public bool IsDrinkable => BuildHP == MaxHP;

        public float TryDrink(float amount)
        {
            return amount;
        }
    }

    private Context myContext => (Context)this.context;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (this.myContext.IsDrinkable)
        {
            this.sprite.Hide();
            this.sprite1.Show();
        }

        this.label.Text = this.myContext.HP.ToString("#") + " / " + this.myContext.BuildHP.ToString("#");
    }

    public override void InitContext(Map.Context mapContext)
    {
        this.context = this.context ?? new Context(10);
        this.context.BlockingCells = new Vector2[]{
            Vector2.Up,
            Vector2.Down,
            Vector2.Left,
            Vector2.Left + Vector2.Up,
            Vector2.Left + Vector2.Down,
            Vector2.Right + Vector2.Up,
            Vector2.Right + Vector2.Down,
        };
        base.InitContext(mapContext);
    }
}
