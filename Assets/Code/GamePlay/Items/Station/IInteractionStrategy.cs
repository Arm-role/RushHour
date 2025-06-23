public interface IInteractionStrategy
{
    bool CanExecute(InteractableItem source, Station targetStation);

    void Execute(InteractableItem source, Station targetStation);
}