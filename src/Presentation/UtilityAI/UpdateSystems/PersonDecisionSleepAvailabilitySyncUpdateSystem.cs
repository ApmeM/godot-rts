using System.Collections.Generic;
using LocomotorECS;

public class PersonDecisionSleepAvailabilitySyncUpdateSystem : MatcherEntitySystem
{
    public PersonDecisionSleepAvailabilitySyncUpdateSystem() : base(new Matcher()
        .All<PersonDecisionSleepComponent>())
    {
    }

    protected override void OnEntityListChanged(HashSet<Entity> added, HashSet<Entity> changed, HashSet<Entity> removed)
    {
        base.OnEntityListChanged(added, changed, removed);

        foreach (var entity in removed)
        {
            entity.GetComponent<PersonDecisionSleepComponent>().SelectedHouse?.GetComponent<AvailabilityComponent>().CurrentUsers.Remove(entity);
            entity.GetComponent<PersonDecisionSleepComponent>().SelectedHouse = null;
        }
    }
    
    protected override void DoAction(float delta)
    {
    }
}
