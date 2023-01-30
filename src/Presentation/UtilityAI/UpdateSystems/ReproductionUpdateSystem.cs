using System;
using System.Linq;
using LocomotorECS;

public class ReproductionUpdateSystem : MatcherEntitySystem
{
    private readonly EntityList el;
    private readonly Random r = new Random();

    public ReproductionUpdateSystem(EntityList el) : base(new Matcher()
        .All<RestComponent>()
        .All<AvailabilityComponent>())
    {
        this.el = el;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var avaiability = entity.GetComponent<AvailabilityComponent>();
        if (avaiability.CurrentUsers.Count() <= 1)
        {
            return;
        }

        if (r.NextDouble() < 0.025)
        {
            var person = Entities.BuildPerson();
            person.GetComponent<PositionComponent>().Position = entity.GetComponent<PositionComponent>().Position;
            el.Add(person);
        }
    }
}
