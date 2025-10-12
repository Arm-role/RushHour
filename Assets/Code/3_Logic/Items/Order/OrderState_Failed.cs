using System.Threading.Tasks;
using UnityEngine;

public class OrderState_Failed : IOrderState
{
    public void OnEnter(OrderLifecycleManager context)
    {
        Debug.Log("ORDER FAILED! Timed out.");

        context.CollectedItems.Clear();

        EventManager.Invoke(new PlateServeToOrder(context.OrderStation));
        EventManager.Invoke(new OrderExpiredEvent(1, context.RequirementData.MenuId));
    }
    public void OnUpdate(OrderLifecycleManager context)
    {
        context.CurrentTime -= Time.deltaTime;
    }
    public Task<bool> HandleInteraction(OrderLifecycleManager context, InteractableItem source) => Task.FromResult(true);
    public void OnExit(OrderLifecycleManager context) { }
    public void OnIngredientAdded(OrderLifecycleManager context, Item ingredient) { }
    public void OnIngredientRemoved(OrderLifecycleManager context, Item ingredient) { }
}
