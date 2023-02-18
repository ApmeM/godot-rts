using LocomotorECS;

public class PlayerComponent : Component
{
    private int playerId;

    public int PlayerId
    {
        get => playerId;
        set
        {
            if (playerId == value)
            {
                return;
            }
            
            playerId = value;
            this.NotifyCustomPropertyChanged();
        }
    }
}
