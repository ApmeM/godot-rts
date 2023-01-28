using System.Collections.Generic;
using LocomotorECS;

public class FatigueSleepThristingSyncUpdateSystem : MatcherEntitySystem
{
    public FatigueSleepThristingSyncUpdateSystem() : base(new Matcher()
        .All<FatigueSleepComponent>())
    {
    }

    protected override void OnEntityListChanged(HashSet<Entity> added, HashSet<Entity> changed, HashSet<Entity> removed)
    {
        base.OnEntityListChanged(added, changed, removed);
        foreach (var entity in added)
        {
            Godot.GD.Print("Sleeping, no thristing.");
            entity.GetComponent<DrinkThristingComponent>()?.Disable();
        }

        foreach (var entity in removed)
        {
            Godot.GD.Print("Waking up, thristing again.");
            entity.GetComponent<DrinkThristingComponent>()?.Enable();
        }
    }

    protected override void DoAction(float delta)
    {
    }
}
