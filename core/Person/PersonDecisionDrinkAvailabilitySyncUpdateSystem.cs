using System.Collections.Generic;
using LocomotorECS;

public class PersonDecisionDrinkAvailabilitySyncUpdateSystem : MatcherEntitySystem
{
    public PersonDecisionDrinkAvailabilitySyncUpdateSystem() : base(new Matcher()
        .All<PersonDecisionDrinkComponent>())
    {
    }

    protected override void OnEntityListChanged(HashSet<Entity> added, HashSet<Entity> changed, HashSet<Entity> removed)
    {
        base.OnEntityListChanged(added, changed, removed);

        foreach (var entity in removed)
        {
            entity.GetComponent<PersonDecisionDrinkComponent>().SelectedSource?.GetComponent<AvailabilityComponent>().CurrentUsers.Remove(entity);
            entity.GetComponent<PersonDecisionDrinkComponent>().SelectedSource = null;
        }
    }
    
    protected override void DoAction(float delta)
    {
    }
}
