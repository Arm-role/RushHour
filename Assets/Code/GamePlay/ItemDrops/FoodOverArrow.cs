using UnityEngine;

public abstract class FoodOverArrow : IDrop
{
    protected virtual int IndexNearPlayer { get; set; }
    public DropExecutionResult Execute(InteractableItem intercatableItem)
    {
        var result = new DropExecutionResult();
        var state = new ProcessState<InteractableItem, TransportItem>();

        result.ParticleToPlay = "Smoke";

        state.Act = (interactable, transportItem) =>
        {
            if (interactable != null && transportItem != null)
            {
                transportItem.Transport(IndexNearPlayer, interactable.Item.Name);
            }
        };
        result.SourceInteraction = (source) =>
        {
            if (source.TryGetComponent<InteractableItem>(out var interactable))
            {
                state.Source = interactable;
                state.Act(state.Source, state.Target);
            }
            else
            {
                Debug.LogWarning("Not Found Source");
            }
        };
        result.TargetInteraction = (target) =>
        {
            if (target.TryGetComponent<TransportItem>(out var trans))
            {
                state.Target = trans;
                state.Act(state.Source, state.Target);
            }
            else
            {
                Debug.LogWarning("Not Found Target");
            }
        };

        return result;
    }
}
