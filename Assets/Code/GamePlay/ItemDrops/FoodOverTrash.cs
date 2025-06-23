public class FoodOverTrash : IDrop
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
