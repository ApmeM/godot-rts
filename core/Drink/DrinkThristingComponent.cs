using LocomotorECS;

public class DrinkThristingComponent : Component
{
    public float ThristThreshold = 50;

    public float CurrentThristing = 100;

    public float MaxThristLevel = 100;

    public float ThristSpeed = 3f;

    public float DrinkSpeed = 50f;
}
