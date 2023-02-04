using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;
using LocomotorECS;

[SceneReference("SelectedActions.tscn")]
public partial class SelectedActions
{
    private World world;
    private MatcherEntityList selectedList;
    private bool enititiesCountChanged = true;

    public World World
    {
        get => world;
        set
        {
            world = value;
            selectedList = new MatcherEntityList(world.el, new Matcher().All<SelectedComponent>());
            selectedList.EntityListChanged += (a, b, c) => this.enititiesCountChanged = this.enititiesCountChanged || a.Any() || c.Any();
        }
    }

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (enititiesCountChanged)
        {
            enititiesCountChanged = false;
            this.actions.ClearChildren();

            if (this.selectedList.Entities.Any())
            {
                return;
            }

            var commonActions = new List<EntityTypeComponent.EntityTypes>{
                EntityTypeComponent.EntityTypes.ArtificialWell,
                EntityTypeComponent.EntityTypes.House
            };

            foreach (var action in commonActions)
            {
                var b = new Button();
                b.Text = action.ToString();
                b.ActionMode = BaseButton.ActionModeEnum.Press;
                b.Connect(CommonSignals.Pressed, this, nameof(StartDrag), new Godot.Collections.Array { action });
                this.actions.AddChild(b);
            }
        }
    }

    private void StartDrag(EntityTypeComponent.EntityTypes type)
    {
        var e = new Entity();
        e.GetOrCreateComponent<EntityTypeComponent>().EntityType = EntityTypeComponent.EntityTypes.House;
        e.AddComponent<PositionComponent>();
        e.AddComponent<MouseInputComponent>();
        e.AddComponent<FollowMouseComponent>();
        e.AddComponent<BindToMapComponent>();

        world.el.Add(e);
    }
}
