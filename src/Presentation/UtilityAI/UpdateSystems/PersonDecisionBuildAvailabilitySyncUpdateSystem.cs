using System.Collections.Generic;
using LocomotorECS;

public class PersonDecisionBuildAvailabilitySyncUpdateSystem : MatcherEntitySystem
{
    public PersonDecisionBuildAvailabilitySyncUpdateSystem() : base(new Matcher()
        .All<PersonDecisionBuildComponent>())
    {
    }

    protected override void OnEntityListChanged(HashSet<Entity> added, HashSet<Entity> changed, HashSet<Entity> removed)
    {
        base.OnEntityListChanged(added, changed, removed);

        foreach (var entity in removed)
        {
            entity.GetComponent<PersonDecisionBuildComponent>().SelectedConstruction?.GetComponent<AvailabilityComponent>().CurrentUsers.Remove(entity);
            entity.GetComponent<PersonDecisionBuildComponent>().SelectedConstruction = null;
        }
    }

    protected override void DoAction(float delta)
    {
    }
}
