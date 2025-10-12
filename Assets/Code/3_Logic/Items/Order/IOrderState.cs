using System.Threading.Tasks;

public interface IOrderState
{
    void OnEnter(OrderLifecycleManager context);
    void OnExit(OrderLifecycleManager context);
    void OnUpdate(OrderLifecycleManager context);
    Task<bool> HandleInteraction(OrderLifecycleManager context, InteractableItem source);
    void OnIngredientAdded(OrderLifecycleManager context, Item ingredient);
    void OnIngredientRemoved(OrderLifecycleManager context, Item ingredient);
}
