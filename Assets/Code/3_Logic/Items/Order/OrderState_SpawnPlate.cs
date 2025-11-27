using System;
using UnityEngine;
using ItemEvents;
using System.Threading.Tasks;
using System.Linq;

public class OrderState_SpawnPlate : IOrderState
{
    private Guid _sessionId;
    public void OnEnter(OrderLifecycleManager context)
    {
    }

    public Task<bool> HandleInteraction(OrderLifecycleManager context, InteractableItem source)
    {
        _sessionId = Guid.NewGuid();

        Action<PlateReadyForOrder> onPlateReadyHandler = null;
        onPlateReadyHandler = (evt) =>
        {
            if (_sessionId == evt.OrderId)
            {
                if (evt.Plate is InteractableItem interactable &&
                interactable.TryGetComponent<Station>(out var plateStation))
                {
                    var itemContainer = plateStation.GetData<ItemContainerData>();

                    EventManager.Unsubscribe(onPlateReadyHandler);
                    context.LinkedPlate = plateStation;
                    var foodRequest = context.RequirementData.RequestItems.Select(item => item.Name);
                    itemContainer.FoodRequest = foodRequest.ToArray();

                    context.SetState(new OrderState_Assembling());
                }
            }
        };

        EventManager.Subscribe(onPlateReadyHandler);
        EventManager.Invoke(new ItemSpawnRequested("Plate", context.OrderStation.transform.position, _sessionId));
        EventManager.Invoke(new OrderSpawnPlate(context.OrderStation));

        return Task.FromResult(true);
    }
    public void OnUpdate(OrderLifecycleManager context)
    {
        context.CurrentTime -= Time.deltaTime;

        if (context.CurrentTime <= 0)
        {
            context.SetState(new OrderState_Failed());
        }
    }
    public void OnExit(OrderLifecycleManager context) { }
    public void OnIngredientAdded(OrderLifecycleManager context, Item ingredient) { }
    public void OnIngredientRemoved(OrderLifecycleManager context, Item ingredient) { }
}