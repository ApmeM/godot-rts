using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;
using Leopotam.EcsLite;

[SceneReference("SelectedActions.tscn")]
public partial class SelectedActions
{
    private static readonly List<EntityTypeComponent.EntityTypes> commonActions = new List<EntityTypeComponent.EntityTypes>{
                EntityTypeComponent.EntityTypes.ArtificialWell,
                EntityTypeComponent.EntityTypes.House
            };
    [Export]
    public int PlayerId;

    public World World;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
/*
        if (enititiesCountChanged)
        {
            enititiesCountChanged = false;
            this.actions.ClearChildren();

            if (this.selectedList.Entities.Any())
            {
                return;
            }


            foreach (var action in commonActions)
            {
                var b = new Button();
                b.Text = action.ToString();
                b.ActionMode = BaseButton.ActionModeEnum.Press;
                b.Connect(CommonSignals.Pressed, this, nameof(StartDrag), new Godot.Collections.Array { action });
                this.actions.AddChild(b);
            }
        }
        */
    }
/*
    private void StartDrag(EntityTypeComponent.EntityTypes type)
    {
        var e = new Entity();
        e.AddComponent<EntityTypeComponent>().EntityType = type;
        e.AddComponent<PositionComponent>();
        e.AddComponent<MouseInputComponent>();
        e.AddComponent<FollowMouseComponent>();
        e.AddComponent<BindToMapComponent>();
        e.AddComponent<SelectPositionMouseComponent>();
        e.AddComponent<PlayerComponent>().PlayerId = this.PlayerId;

        world.el.Add(e);
    }
*/
}
