using System;

public enum Notifications
{
    None,
    ThristingDead,
    SleepingOnTheGround,
    ConstructionComplete
}

public static class NotificationsExtensions
{
    public static string ToReadableText(this Notifications notification)
    {
        switch (notification)
        {
            case Notifications.None:
                {
                    return "";
                }
            case Notifications.SleepingOnTheGround:
                {
                    return $"Your people are sleeping on the ground. Build more houses.";
                }
            case Notifications.ThristingDead:
                {
                    return $"Your people are thristing and dying. Build more wells.";
                }
            case Notifications.ConstructionComplete:
                {
                    return $"Construction complete.";
                }
            default:
                {
                    throw new Exception($"Not translated notification type {notification}");
                }
        }
    }

}