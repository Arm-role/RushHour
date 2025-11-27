using System.Threading.Tasks;
using UnityEngine;

public class WareOverTrash : IDrop
{
    public DropExecutionResult Execute(InteractableItem intercatableItem)
    {
        var result = new DropExecutionResult
        {
            ShouldDestroySelf = Task.FromResult(true),
            SFXPlay = "Pop",
            ParticleToPlay = "Smoke"
        };
        return result;
    }
}
