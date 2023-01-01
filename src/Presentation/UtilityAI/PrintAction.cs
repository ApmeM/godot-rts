using BrainAI.AI.UtilityAI;

public interface IPrintActionContext
{
    string CurrentActionName { get; set; }
}

public class PrintAction<T> : IAction<T> where T : IPrintActionContext
{
    private string text;

    public PrintAction(string text)
    {
        this.text = text;
    }

    public void Enter(T context)
    {
    }

    public void Execute(T context)
    {
        context.CurrentActionName = text;
    }

    public void Exit(T context)
    {
    }
}

