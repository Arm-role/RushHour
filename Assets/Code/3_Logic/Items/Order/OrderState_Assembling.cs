using UnityEngine;
using GameEvents;
using System.Threading.Tasks;
using ItemEvents;
using System.Linq;
public class OrderState_Assembling : IOrderState
{
    public void OnEnter(OrderLifecycleManager context) { }
    public void OnIngredientAdded(OrderLifecycleManager context, Item ingredient)
    {
        bool required = context.RequirementData.RequiredItemAndCounts
        .Any(list => list.Contains(ingredient));

        if (required)
        {
            context.CollectedItems.Add(ingredient);

            var ingredientAddToOrder = new IngredienAddToOrder(
                context.OrderStation,
                context.GetFirstItemRequest()
            );

            EventManager.Invoke(ingredientAddToOrder);
        }
    }
    public void OnIngredientRemoved(OrderLifecycleManager context, Item ingredient)
    {
        if (context.CollectedItems.Contains(ingredient))
        {
            context.CollectedItems.Remove(ingredient);
        }
    }
    public void OnUpdate(OrderLifecycleManager context)
    {
        context.CurrentTime -= Time.deltaTime;

        if (context.CurrentTime <= 0)
        {
            context.SetState(new OrderState_Failed());
        }
    }
    public Task<bool> HandleInteraction(OrderLifecycleManager context, InteractableItem source)
    {
        if (source != null && source.TryGetComponent<Station>(out var plate) && plate == context.LinkedPlate)
        {
            if (context.CollectedItems.Count == context.RequirementData.RequestItems.Count)
            {
                context.SetState(new OrderState_Fulfilled());
                return Task.FromResult(true);
            }
            else
            {
                Debug.Log(context.CollectedItems.Count + " : " + context.RequirementData.RequestItems.Count);
                EventManager.Invoke(new PlaySound("ErrorSound"));
            }
        }
        return Task.FromResult(false);
    }
    public void OnExit(OrderLifecycleManager context) { }

   
}