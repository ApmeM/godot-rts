using System;
using Godot;
using LocomotorECS;

public class WalkingUpdateSystem : MatcherEntitySystem
{
    private Random r = new Random();

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

        if (moving.PathTarget != Godot.Vector2.Inf)
        {
            return;
        }

        moving.PathTarget = position.Position + new Vector2(r.Next(250) - 125, r.Next(250) - 125);
    }
}
