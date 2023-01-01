using System;
using BrainAI.AI.UtilityAI;
using Godot;

public interface IDrinkFromActionContext
{
    float CurrentAmount { get; }
    Vector2 Position { get; }
    bool IsDrinkable { get; }
    float TryDrink(float amount);
}

public interface IDrinkActionContext
{
    float DrinkSpeed { get; }
    float Delta { get; }
    float MaxThristLevel { get; }
    float CurrentThristLevel { get; }
    IDrinkFromActionContext Well { get; }
    void Drink(float amount);
}

public class DrinkAction<T> : IAction<T> where T : IDrinkActionContext
{
    public DrinkAction()
    {
    }

    public void Enter(T context)
    {
    }

    public void Execute(T context)
    {
        var toDrink = Math.Min(context.DrinkSpeed * context.Delta, context.MaxThristLevel - context.CurrentThristLevel);
        var drinkAmount = context.Well.TryDrink(toDrink);
        context.Drink(drinkAmount);
    }

    public void Exit(T context)
    {
    }
}

