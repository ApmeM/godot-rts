using GodotAnalysers;
using Godot;

[SceneReference("Construction.tscn")]
public partial class Construction
{
    public class Context : TileMapObject.Context, IBuildItemActionContext
    {
        public Context(float maxHP)
        {
            this.MaxHP = maxHP;
        }
        public float MaxHP { get; private set; }
        public float BuildHP { get; private set; }
        public float HP { get; private set; }

        public virtual void BuildComplete()
        {

        }

        public void Build(float hp)
        {
            BuildHP += hp;
            HP += hp;
            if (BuildHP >= MaxHP)
            {
                var delta = BuildHP - MaxHP;
                BuildHP -= delta;
                HP -= delta;
                this.MapContext.RemoveItemByType(Map.Context.MapItemType.Construction, this);
                BuildComplete();
            }
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

        this.label.Text = this.myContext.HP.ToString("#") + " / " + this.myContext.BuildHP.ToString("#");
    }

    public override void InitContext(Map.Context mapContext)
    {
        this.context = this.context ?? new Context(1);
        base.InitContext(mapContext);
        mapContext.AddItemByType(Map.Context.MapItemType.Construction, this.context);
    }
}
