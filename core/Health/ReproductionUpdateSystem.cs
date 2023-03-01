using System;
using LocomotorECS;

public class ReproductionUpdateSystem : MatcherEntitySystem
{
    private readonly EntityList el;
    private readonly Random r = new Random();

    public ReproductionUpdateSystem(EntityList el) : base(new Matcher()
        .All<RestComponent>()
        .All<PlayerComponent>()
        .All<AvailabilityComponent>())
    {
        this.el = el;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var avaiability = entity.GetComponent<AvailabilityComponent>();
        if (avaiability.CurrentUsers.Count <= 1)
        {
            return;
        }

        if (r.NextDouble() < 0.025)
        {
            var player = entity.GetComponent<PlayerComponent>();
            var person = Entities.Build(EntityTypeComponent.EntityTypes.Person, player.PlayerId);
            person.GetComponent<PositionComponent>().Position = entity.GetComponent<PositionComponent>().Position;
            el.Add(person);
        }
    }
}
