using System;
using System.Collections.Generic;

public class ActionExecutor
{
    private Queue<IUnitAction> PendingActions = new Queue<IUnitAction>();

    public void CancelActions()
    {
        this.PendingActions.Clear();
    }

    public void AddAction(IUnitAction action)
    {
        this.PendingActions.Enqueue(action);
    }

    public void Process(float delta)
    {
        if (PendingActions.Count <= 0)
        {
            return;
        }

        var action = PendingActions.Peek();
        var isActionDone = action.Process(delta);
        if (isActionDone)
        {
            PendingActions.Dequeue();
        }
    }

    public bool IsEmpty()
    {
        return PendingActions.Count == 0;
    }
}