using System.Linq;
using LocomotorECS;

public class BuildProcessUpdateSystem : MatcherEntitySystem
{
    private EntityLookup<int> constructionSource;

    public BuildProcessUpdateSystem(EntityLookup<int> constructionSource) : base(new Matcher()
        .All<PersonDecisionBuildComponent>()
        .All<BuilderComponent>()
        .All<PositionComponent>()
        .All<PlayerComponent>()
        .Exclude<FatigueSleepComponent>())
    {
        this.constructionSource = constructionSource;
    }

    protected override void DoAction(float delta)
    {
        if (!constructionSource.Any(a => a.Value.Entities.Any()))
        {
            return;
        }

        base.DoAction(delta);
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var position = entity.GetComponent<PositionComponent>();
        var player = entity.GetComponent<PlayerComponent>();

        var closestSource = constructionSource[player.PlayerId].Entities
                            .Where(a => a.GetComponent<AvailabilityComponent>()?.IsAvailable(entity) ?? true)
                            .OrderBy(a => (a.GetComponent<PositionComponent>().Position - position.Position).LengthSquared())
                            .FirstOrDefault();
        var closestConstruction = closestSource?.GetComponent<PositionComponent>()?.Position ?? Godot.Vector2.Inf;

        if (position.Position != closestConstruction)
        {
            entity.GetComponent<PersonDecisionBuildComponent>().SelectedConstruction?.GetComponent<AvailabilityComponent>().CurrentUsers.Remove(entity);
            entity.GetComponent<PersonDecisionBuildComponent>().SelectedConstruction = null;
            return;
        }

        var construction = closestSource.GetComponent<ConstructionComponent>();
        if (construction.BuildProgress >= 1)
        {
            construction.ConstructionDone?.Invoke(closestSource);
            construction.ConstructionDone = null;
            closestSource.RemoveComponent<ConstructionComponent>();
            return;
        }

        closestSource.GetOrCreateComponent<AvailabilityComponent>()?.CurrentUsers.Add(entity);
        entity.GetComponent<PersonDecisionBuildComponent>().SelectedConstruction = closestSource;

        var builder = entity.GetComponent<BuilderComponent>();

        var hpToPct = 0.01f;
        var hp = closestSource.GetComponent<HPComponent>();
        if (hp != null)
        {
            hpToPct = 1f / hp.MaxHP;
        }
        var buildProgress = builder.BuildSpeed * hpToPct * delta;
        construction.BuildProgress += buildProgress;
        if (hp != null)
        {
            hp.HP += builder.BuildSpeed;
            if (hp.HP > hp.MaxHP)
            {
                hp.HP = hp.MaxHP;
            }
        }
    }
}
