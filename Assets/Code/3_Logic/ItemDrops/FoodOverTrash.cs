using System.Threading.Tasks;

public class FoodOverTrash : IDrop
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
