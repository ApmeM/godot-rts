using LocomotorECS;

public class EntityTypeComponent : Component
{
    public enum EntityTypes
    {
        ArtificialWell = 1,
        House = 2,
        Person = 3,
        Tree = 4,
        Well = 5
    }

    public EntityTypes EntityType;
}
