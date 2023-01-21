using LocomotorECS;

public class DrinkThristingComponent : Component
{
    public float ThristThreshold { get; set; } = 50;

    public float CurrentThristing { get; set; } = 100;

    public float MaxThristLevel { get; set; } = 100;

    public float ThristSpeed { get; set; } = 3f;

    public float DrinkSpeed { get; set; } = 50f;
}
