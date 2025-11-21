using UnityEngine;
using GameEvents;
using System.Threading.Tasks;
public class OrderState_Fulfilled : IOrderState
{
    public void OnEnter(OrderLifecycleManager context)
    {
        EventManager.Invoke(new PlateServeToOrder(context.OrderStation));
        EventManager.Invoke(new SentMenu(context.RequirementData.MenuId, context.RequirementData.ScoreValue));

        context.CollectedItems.Clear();
    }
    public void OnUpdate(OrderLifecycleManager context) { }
    public Task<bool> HandleInteraction(OrderLifecycleManager context, InteractableItem source) => Task.FromResult(true);
    public void OnExit(OrderLifecycleManager context) { }
    public void OnIngredientAdded(OrderLifecycleManager context, Item ingredient) { }
    public void OnIngredientRemoved(OrderLifecycleManager context, Item ingredient) { }
}
