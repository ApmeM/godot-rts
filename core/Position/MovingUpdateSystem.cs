using System.Numerics;
using LocomotorECS;

public class MovingUpdateSystem : MatcherEntitySystem
{
    private readonly GameContext gameContext;

    public MovingUpdateSystem(GameContext gameContext) : base(new Matcher()
        .All<PositionComponent>()
        .All<MovingComponent>()
        .Exclude<FatigueSleepComponent>())
    {
        this.gameContext = gameContext;
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);
        var position = entity.GetComponent<PositionComponent>();
        var moving = entity.GetComponent<MovingComponent>();

        if (moving.PathTarget == Vector2Ext.Inf)
        {
            return;
        }

        if (moving.Path.Count == 0 || moving.PathTarget != moving.Path[moving.Path.Count - 1])
        {
            moving.Path.Clear();

            var newPath = this.gameContext.FindPath(position.Position, moving.PathTarget);
            if (newPath == null)
            {
                moving.PathTarget = Vector2Ext.Inf;
                return;
            }

            moving.Path.AddRange(newPath);
            moving.PathTarget = moving.Path[moving.Path.Count - 1];
        }

        var path = moving.Path[0] - position.Position;
        var motion = path.Normalized() * moving.MoveSpeed * delta;
        if (path.LengthSquared() > motion.LengthSquared())
        {
            position.Position += motion;
        }
        else
        {
            position.Position = moving.Path[0];
            moving.Path.RemoveAt(0);
            if (moving.Path.Count == 0)
            {
                moving.PathTarget = Vector2Ext.Inf;
            }
        }
    }
}
