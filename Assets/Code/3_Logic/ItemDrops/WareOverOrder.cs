using UnityEngine;

public class WareOverOrder : IDrop
{
    public DropExecutionResult Execute(InteractableItem intercatableItem)
    {
        var result = new DropExecutionResult();
        var state = new ProcessState<InteractableItem, Station>();

        result.ParticleToPlay = "Smoke";

        state.Func = async (interactable, station) =>
        {
            if (interactable != null && station != null)
            {
                return await station.Interact(interactable);
            }
            return false;
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
            if (target.TryGetComponent<Station>(out var station))
            {
                state.Target = station;
                var shouldDestroy = state.Func(state.Source, state.Target);
                result.ShouldDestroySelf = shouldDestroy;
                result.ShouldDestroyTarget = shouldDestroy;
            }
        };

        return result;
    }
}