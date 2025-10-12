using UnityEngine;

public class OrderQuest_Work : IWorkStation
{
    private OrderLifecycleManager _orderLifeCycle;
    public OrderQuest_Work(OrderLifecycleManager orderLifecycle)
    {
        _orderLifeCycle = orderLifecycle;
    }
    public void OnStart(Station station)
    {
        EventManager.Invoke(new WorkStarted(station));
    }
    public void OnUpdate(Station station)
    {
        _orderLifeCycle.Update();
        EventManager.Invoke(new WorkProgress(station));
    }

    public void OnRecieveExternalInput(Station station) { }
    public bool IsComplete(Station station)
    {
        return _orderLifeCycle.CurrentState is OrderState_Fulfilled;
    }

    public void OnComplete(Station station)
    {
        EventManager.Invoke(new WorkCompleted(station));
    }

    public bool IsCancel(Station station)
    {
        return _orderLifeCycle.CurrentState is OrderState_Failed;
    }

    public void OnCancel(Station station)
    {
        EventManager.Invoke(new WorkCanceled(station));
        Debug.Log("Cancel Work");

        if (station == null)
        {
            Debug.LogError("❌ OnCancel() failed: station is null");
            return;
        }

        if (!station.TryGetData<OrderLifecycleManager>(out var result))
        {
            Debug.LogError($"❌ OnCancel() failed: {station.name} has no OrderLifecycleManager data.");
            return;
        }

        if (result == null)
        {
            Debug.LogError("❌ OnCancel() failed: result (OrderLifecycleManager) is null.");
            return;
        }

        if (result.OrderStation != null)
        {
            if (result.OrderStation.TryGetComponent<InteractableItem>(out var order))
                order.RequestDestruction();
            else
                Debug.LogWarning("⚠️ OnCancel: OrderStation has no InteractableItem");
        }

        if (result.LinkedPlate != null)
        {
            if (result.LinkedPlate.TryGetComponent<InteractableItem>(out var plate))
                plate.RequestDestruction();
            else
                Debug.LogWarning("⚠️ OnCancel: LinkedPlate has no InteractableItem");
        }
    }
}