using System;
using System.Numerics;
using LocomotorECS;

public class WalkingUpdateSystem : MatcherEntitySystem
{
    private readonly Random r = new Random();

    public WalkingUpdateSystem() 
        : base(new Matcher()
            .All<PersonDecisionWalkComponent>()
            .All<MovingComponent>()
            .All<PositionComponent>()
            .Exclude<FatigueSleepComponent>())
    {
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var moving = entity.GetComponent<MovingComponent>();
        var position = entity.GetComponent<PositionComponent>();

        if (moving.PathTarget != Vector2Ext.Inf)
        {
            return;
        }

        moving.PathTarget = position.Position + new Vector2(r.Next(250) - 125, r.Next(250) - 125);
    }
}
