using LocomotorECS;

public class EntityTypeComponent : Component
{
    public enum EntityTypes
    {
        ArtificialWell,
        House,
        Person,
        Tree,
        Well
    }

    public EntityTypes EntityType;
}
