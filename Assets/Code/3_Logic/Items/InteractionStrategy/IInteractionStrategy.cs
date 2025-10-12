using System.Threading.Tasks;

public interface IInteractionStrategy
{
    int GetExecutionPriority(InteractableItem source, Station targetStation);
    Task<bool> Execute(InteractableItem source, Station targetStation);
}