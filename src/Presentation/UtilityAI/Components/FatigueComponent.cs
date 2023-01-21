using LocomotorECS;

public class FatigueComponent : Component
{
    public float FatigueThreshold { get; set; }
    public float DefaultRest { get; set; }
    public float CurrentFatigue { get; set; }
    public float MaxFatigue { get; set; }
    public float FatigueSpeed { get; set; }
}
