using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderServe_Interaction", menuName = "InteractionStrategy/OrderServe_Interaction")]
public class OrderServe_Interaction : InteractionStrategy
{
    public override int GetExecutionPriority(InteractableItem source, Station targetStation)
    {
        if (source != null && targetStation.TryGetData<OrderLifecycleManager>(out var orderLifecycle) &&
            source.TryGetComponent<Station>(out var station) && station == orderLifecycle.LinkedPlate)
        {
            return 1;
        }
        return 0;
    }
    public override Task<bool> Execute(InteractableItem source, Station targetStation)
    {
        var orderManager = targetStation.GetData<OrderLifecycleManager>();
        return orderManager.OnInteract(source); 
    }
}