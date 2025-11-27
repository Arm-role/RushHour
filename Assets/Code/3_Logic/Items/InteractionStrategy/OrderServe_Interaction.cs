using System.Threading.Tasks;

public class OrderServe_Interaction : IInteractionStrategy
{
    public int GetExecutionPriority(InteractableItem source, Station targetStation)
    {
        if (source != null && targetStation.TryGetData<OrderLifecycleManager>(out var orderLifecycle) &&
            source.TryGetComponent<Station>(out var station) && station == orderLifecycle.LinkedPlate)
        {
            return 1;
        }
        return 0;
    }
    public Task<bool> Execute(InteractableItem source, Station targetStation)
    {
        var orderManager = targetStation.GetData<OrderLifecycleManager>();
        return orderManager.OnInteract(source);
    }
}