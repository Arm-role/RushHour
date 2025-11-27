using System.Threading.Tasks;

public class OrderLifecycle_Interaction : IInteractionStrategy
{
    public int GetExecutionPriority(InteractableItem source, Station targetStation)
    {
        if (source == null && targetStation.TryGetData<OrderLifecycleManager>(out _))
        {
            return 1;
        }
        return 0;
    }

    public async Task<bool> Execute(InteractableItem source, Station targetStation)
    {
        var orderManager = targetStation.GetData<OrderLifecycleManager>();

        if (orderManager.CurrentState is OrderState_AwaitingActivation)
        {
            var orderQuest_Work = new OrderQuest_Work(orderManager, targetStation);
            targetStation.worker.BeginWork(orderQuest_Work, targetStation);
        }

        return await orderManager.OnInteract(source);
    }
}