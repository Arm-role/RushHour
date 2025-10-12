using System.Threading.Tasks;
using UnityEngine;

public class OrderState_AwaitingActivation : IOrderState
{
    public void OnEnter(OrderLifecycleManager context) { }

    public Task<bool> HandleInteraction(OrderLifecycleManager context, InteractableItem source)
    {
        if (source == null)
        {
            context.SetState(new OrderState_SpawnPlate());
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public void OnUpdate(OrderLifecycleManager context)
    {
        context.CurrentTime -= Time.deltaTime;
    }
    public void OnExit(OrderLifecycleManager context) { }
    public void OnIngredientAdded(OrderLifecycleManager context, Item ingredient) { }
    public void OnIngredientRemoved(OrderLifecycleManager context, Item ingredient) { }
}