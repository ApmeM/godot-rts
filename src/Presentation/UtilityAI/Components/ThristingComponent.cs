using LocomotorECS;

public class ThristingComponent : Component
{
    public float ThristThreshold { get; set; } = 50;

    public float CurrentThristLevel { get; set; } = 100;

    public float MaxThristLevel { get; set; } = 100;

    public float ThristSpeed { get; set; } = 3f;

    public float DrinkSpeed { get; set; } = 50f;
}
