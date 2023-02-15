using System.Linq;
using LocomotorECS;

public class BuildProcessUpdateSystem : MatcherEntitySystem
{
    private MatcherEntityList constructionSource;

    public BuildProcessUpdateSystem() : base(new Matcher()
        .All<PersonDecisionBuildComponent>()
        .All<BuilderComponent>()
        .All<PositionComponent>()
        .Exclude<FatigueSleepComponent>())
    {
    }

    protected override void DoAction(float delta)
    {
        if (!constructionSource.Entities.Any())
        {
            return;
        }

        base.DoAction(delta);
    }

    protected override void DoAction(Entity entity, float delta)
    {
        base.DoAction(entity, delta);

        var position = entity.GetComponent<PositionComponent>();

        var closestSource = constructionSource.Entities
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

    protected override EntityListChangeNotificator FilterEntityList(EntityListChangeNotificator entityList)
    {
        this.constructionSource = new MatcherEntityList(entityList, new Matcher().All<ConstructionComponent>());
        return base.FilterEntityList(entityList);
    }
}
