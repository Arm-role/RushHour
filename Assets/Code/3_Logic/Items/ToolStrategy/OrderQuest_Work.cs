public class OrderQuest_Work : IWorkStation
{
    private OrderLifecycleManager _orderLifecycle;
    private Station _station;
    public OrderQuest_Work(OrderLifecycleManager orderLifecycle, Station station)
    {
        _orderLifecycle = orderLifecycle;
        _station = station;
    }
    public void OnStart(Station station)
    {
        if (station != _station) return;
        EventManager.Invoke(new WorkStarted(station));
    }
    public void OnUpdate(Station station)
    {
        if (station != _station) return;

        _orderLifecycle.Update();
        EventManager.Invoke(new WorkProgress(station));
    }

    public void OnRecieveExternalInput(Station station) { }
    public bool IsComplete(Station station)
    {
        if (station != _station) return false;

        return _orderLifecycle.CurrentState is OrderState_Fulfilled;
    }

    public void OnComplete(Station station)
    {
        if (station != _station) return;

        EventManager.Invoke(new WorkCompleted(station));
    }

    public bool IsCancel(Station station)
    {
        if (station != _station) return false;

        return _orderLifecycle.CurrentState is OrderState_Failed;
    }

    public void OnCancel(Station station)
    {
        if (station != _station) return;

        EventManager.Invoke(new WorkCanceled(station));

        if (_orderLifecycle.OrderStation != null)
        {
            if (_orderLifecycle.OrderStation.TryGetComponent<IDestructible>(out var order))
            {
                order.RequestDestruction();
            }
        }

        if (_orderLifecycle.LinkedPlate == null) return;

        if (_orderLifecycle.LinkedPlate.TryGetComponent<IDestructible>(out var plate))
        {
            plate.RequestDestruction();
        }
    }
}