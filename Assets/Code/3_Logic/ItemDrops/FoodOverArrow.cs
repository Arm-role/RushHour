using System.Threading.Tasks;
using UnityEngine;

public abstract class FoodOverArrow : IDrop
{
    protected virtual int IndexNearPlayer { get; set; }
    public DropExecutionResult Execute(InteractableItem intercatableItem)
    {
        var result = new DropExecutionResult();
        var state = new ProcessState<InteractableItem, TransportItem>();

        result.ParticleToPlay = "Smoke";

        state.Func = (interactable, transportItem) =>
        {
            if (interactable != null && transportItem != null)
            {
                return transportItem.Transport(IndexNearPlayer, interactable.Item.Name);
            }
            return Task.FromResult(false);
        };
        result.SourceInteraction = (source) =>
        {
            if (source.TryGetComponent<InteractableItem>(out var interactable))
            {
                state.Source = interactable;
            }
        };
        result.TargetInteraction = (target) =>
        {
            var trans = target.GetComponentInParent<TransportItem>();
            if (trans != null)
            {
                state.Target = trans;
                result.ShouldDestroySelf = state.Func(state.Source, state.Target);
            }
        };

        return result;
    }
}
