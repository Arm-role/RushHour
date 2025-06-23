using UnityEngine;
public class FoodOverTool : IDrop
{
    public DropExecutionResult Execute(InteractableItem intercatableItem)
    {
        var result = new DropExecutionResult();
        var state = new ProcessState<InteractableItem, Station>();

        result.ParticleToPlay = "Smoke";

        state.Func = (interactable, station) =>
        {
            if (interactable != null && station != null)
            {
                return station.Interact(interactable);
            }
            return false;
        };
        result.SourceInteraction = (source) =>
        {
            if (source.TryGetComponent<InteractableItem>(out var interactable))
            {
                state.Source = interactable;
                result.ShouldDestroySelf = state.Func(state.Source, state.Target);
            }
            else
            {
                Debug.LogWarning("Not Found Source");
            }
        };
        result.TargetInteraction = (target) =>
        {
            if (target.TryGetComponent<Station>(out var station))
            {
                state.Target = station;
                result.ShouldDestroySelf = state.Func(state.Source, state.Target);
            }
            else
            {
                Debug.LogWarning("Not Found Target");
            }
        };

        return result;
    }
}
