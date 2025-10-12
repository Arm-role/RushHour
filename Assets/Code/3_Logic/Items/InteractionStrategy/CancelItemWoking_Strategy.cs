using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CancelItemWoking_Strategy", menuName = "InteractionStrategy/CancelItemWoking_Strategy")]
public class CancelItemWoking_Strategy : InteractionStrategy
{
    public override int GetExecutionPriority(InteractableItem source, Station targetStation)
    {
        if (source == null && targetStation.GetData<ToolWorkData>().IsWorking)
        {
            return 100;
        }
        return 0;
    }
    public override Task<bool> Execute(InteractableItem source, Station targetStation)
    {
        if (targetStation.TryGetData<ToolWorkData>(out var result1))
        {
            result1.IsCancel = true;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
