using BrainAI.AI;
using LocomotorECS;

public class AIComponent : Component
{
    public enum Decision
    {
        None,
        Drink,
        Drinking,
    }

    public Decision SelectedDecision;
    public Decision PreviousDecision;
    public IAITurn AIBot { get; set; }
}

