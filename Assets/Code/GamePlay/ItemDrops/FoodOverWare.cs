using UnityEngine;

public class FoodOverWare : IDrop
{
    public DropExecutionResult Execute(InteractableItem intercatableItem)
    {
        var result = new DropExecutionResult
        {
            ShouldDestroySelf = true,
            ParticleToPlay = "Smoke",
            //TargetInteraction = (targetCollider) =>
            //{
            //    if (targetCollider.TryGetComponent<InteractableItem>(out var interactable) && targetCollider.TryGetComponent<Station>(out var station))
            //    {
            //        station.Interact(interactable);
            //    }
            //    else
            //    {
            //        Debug.LogWarning("Not Found Target");
            //    }
            //}
        };

        return result;
    }
}
