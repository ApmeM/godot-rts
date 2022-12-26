public class SleepUnitAction : IUnitAction
{
    private float delay;

    public SleepUnitAction(float delay)
    {
        this.delay = delay;
    }

    public bool Process(float delta)
    {
        delay -= delta;
        return delay <= 0;
    }
}