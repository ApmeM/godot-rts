using System.Collections.Generic;
using LocomotorECS;

public class RemoveDeadSystem : MatcherEntitySystem
{
    private readonly World world;

    public RemoveDeadSystem(World world) : base(new Matcher().All<DeadComponent>())
    {
        this.world = world;
    }

    protected override void OnEntityListChanged(HashSet<Entity> added, HashSet<Entity> changed, HashSet<Entity> removed)
    {
        base.OnEntityListChanged(added, changed, removed);

        foreach (var entity in added)
        {
            this.world.el.Remove(entity);
        }

        foreach (var entity in removed)
        {
            Entities.Return(entity);
        }
    }
}
