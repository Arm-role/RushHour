using System.Threading.Tasks;

public class CancelItemWoking_Strategy : IInteractionStrategy
{
    public int GetExecutionPriority(InteractableItem source, Station targetStation)
    {
        if (source == null && targetStation.GetData<ToolWorkData>().IsWorking)
        {
            return 100;
        }
        return 0;
    }
    public Task<bool> Execute(InteractableItem source, Station targetStation)
    {
        if (targetStation.TryGetData<ToolWorkData>(out var result1))
        {
            result1.IsCancel = true;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
