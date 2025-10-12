using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderLifecycle_Interaction", menuName = "InteractionStrategy/OrderLifecycle_Interaction")]
public class OrderLifecycle_Interaction : InteractionStrategy
{
    public override int GetExecutionPriority(InteractableItem source, Station targetStation)
    {
        if (source == null && targetStation.TryGetData<OrderLifecycleManager>(out _))
        {
            return 1;
        }
        return 0;
    }

    public override async Task<bool> Execute(InteractableItem source, Station targetStation)
    {
        var orderManager = targetStation.GetData<OrderLifecycleManager>();

        if (orderManager.CurrentState is OrderState_AwaitingActivation)
        {
            var orderQuest_Work = new OrderQuest_Work(orderManager);
            targetStation.worker.BeginWork(orderQuest_Work, targetStation);
        }

        return await orderManager.OnInteract(source); 
    }
}