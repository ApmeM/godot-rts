using System;
using System.Linq;
using Godot;
using GodotAnalysers;
using GodotRts.Presentation.Utils;
using LocomotorECS;

[SceneReference("SelectedDetails.tscn")]
public partial class SelectedDetails
{
    private World world;
    private MatcherEntityList selectedList;
    private bool enititiesCountChanged;

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
            this.stats.ClearChildren();

            if (!this.selectedList.Entities.Any())
            {
                return;
            }

            if (this.selectedList.Entities.Count == 1)
            {
                var e = this.selectedList.Entities.First();
                var label = new Label();
                this.stats.AddChild(label);
                return;
            }

            var id = 0;
            foreach (var e in this.selectedList.Entities)
            {
                var button = new Button();
                button.Text = e.GetComponent<EntityTypeComponent>()?.EntityType.ToString();
                button.Connect(CommonSignals.Pressed, this, nameof(ButtonClicked), new Godot.Collections.Array { id });
                this.stats.AddChild(button);
                id++;
            }
        }
        else if (this.selectedList.Entities.Count == 1 && this.stats.GetChildCount() == 1)
        {
            var e = this.selectedList.Entities.First();
            var label = this.stats.GetChild<Label>(0);
            var entityType = e.GetComponent<EntityTypeComponent>();
            var construction = e.GetComponent<ConstructionComponent>();
            var availability = e.GetComponent<AvailabilityComponent>();

            if (construction?.Enabled ?? false)
            {
                label.Text = $"Progress: {(construction.BuildProgress * 100), 0}%\nBuilders: {availability.CurrentUsers.Count} / {availability.MaxNumberOfUsers}";

                return;
            }

            switch (entityType?.EntityType)
            {
                case EntityTypeComponent.EntityTypes.ArtificialWell:
                    label.Text = e.GetComponent<DrinkableComponent>().CurrentAmount.ToString("#") + "\n" +
                        $"{availability.CurrentUsers.Count} / {availability.MaxNumberOfUsers}";
                    break;
                case EntityTypeComponent.EntityTypes.House:
                    label.Text = $"Availability: {availability.CurrentUsers.Count} / {availability.MaxNumberOfUsers}";
                    break;
                case EntityTypeComponent.EntityTypes.Person:
                    label.Text = e.GetComponent<PrintComponent>().Text + "\n" +
                        "Thristing: " + e.GetComponent<DrinkThristingComponent>().CurrentThristing.ToString("#") + "\n" +
                        "Fatigue: " + e.GetComponent<FatigueComponent>().CurrentFatigue.ToString("#") + "\n";
                    break;
                case EntityTypeComponent.EntityTypes.Tree:
                    break;
                case EntityTypeComponent.EntityTypes.Well:
                    label.Text = e.GetComponent<DrinkableComponent>().CurrentAmount.ToString("#");
                    break;
                default:
                    break;
            }
        }
    }

    private void ButtonClicked(int selectedId)
    {
        var id = 0;
        foreach (var e in this.selectedList.Entities)
        {
            e.GetComponent<SelectedComponent>().Enabled = selectedId == id;
            id++;
        }
    }
}
