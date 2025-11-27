using ItemEvents;
using System;
using System.Collections.Generic;

public class OrderOrchestratorSystem
{
    private List<OrderLifecycleManager> _activeOrders = new List<OrderLifecycleManager>();

    public OrderOrchestratorSystem(Action OnDestroy)
    {
        EventManager.Subscribe<ItemSpawned>(CheckForNewOrder);
        EventManager.Subscribe<ItemAddToStation>(RouteIngredientToOrder);
        EventManager.Subscribe<ItemRemoveFromStation>(RouteIngredientToRemove);
        EventManager.Subscribe<PlateServeToOrder>(RemoveOrderContext);

        OnDestroy += Dispose;
    }

    private void Dispose()
    {
        EventManager.Unsubscribe<ItemSpawned>(CheckForNewOrder);
        EventManager.Unsubscribe<ItemAddToStation>(RouteIngredientToOrder);
        EventManager.Unsubscribe<ItemRemoveFromStation>(RouteIngredientToRemove);
        EventManager.Unsubscribe<PlateServeToOrder>(RemoveOrderContext);
    }

    private void CheckForNewOrder(ItemSpawned evt)
    {
        if (evt.Interactable is not InteractableItem interactable) return;

        if (interactable.TryGetComponent<Station>(out var station) &&
            station.TryGetData<OrderLifecycleManager>(out var manager) && 
            !_activeOrders.Contains(manager))
        {
            _activeOrders.Add(manager);
        }
    }

    private void RouteIngredientToOrder(ItemAddToStation evt)
    {
        foreach (var order in _activeOrders)
        {
            if (order.LinkedPlate == evt.Station)
            {
                order.OnIngredientAddedToPlate(evt.Item);
                return;
            }
        }
    }
    private void RouteIngredientToRemove(ItemRemoveFromStation evt)
    {
        foreach (var order in _activeOrders)
        {
            if (order.LinkedPlate == evt.Station)
            {
                order.OnIngredientRemoveFromPlate(evt.Item);
                return;
            }
        }
    }

    private void RemoveOrderContext(PlateServeToOrder evt)
    {
        var manager = evt.Station.GetData<OrderLifecycleManager>();

        if (_activeOrders.Contains(manager))
        {
            _activeOrders.Remove(manager);
        }
    }
}
