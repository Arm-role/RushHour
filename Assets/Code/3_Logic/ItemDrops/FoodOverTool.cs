using UnityEngine;
public class FoodOverTool : IDrop
{
    public DropExecutionResult Execute(InteractableItem intercatableItem)
    {
        var result = new DropExecutionResult();

        InteractableItem interactable = null;
        bool haveSource = false;
        bool haveTarget = false;

        result.SourceInteraction = (source) =>
        {
            haveSource = source.TryGetComponent(out interactable);
        };  
        result.TargetInteraction = (target) =>
        {
            haveTarget = target.TryGetComponent<Station>(out var station);

            if (haveSource && haveTarget)
            {
                result.SFXPlay = "Pop";
                result.ParticleToPlay = "Smoke";
                result.ShouldDestroySelf = station.Interact(interactable);
            }
        };

        return result;
    }
}