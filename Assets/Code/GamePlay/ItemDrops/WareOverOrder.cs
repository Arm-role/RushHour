using UnityEngine;

public class WareOverOrder : IDrop
{
    public DropExecutionResult Execute(InteractableItem intercatableItem)
    {
        var result = new DropExecutionResult
        {
            ShouldDestroySelf = true,
            ParticleToPlay = "Smoke"
        };

        return result;
    }
}