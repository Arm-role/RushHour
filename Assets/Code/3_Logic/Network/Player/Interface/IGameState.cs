
// --- Game State Interface ---
public interface IGameState
{
    int TotalScore { get; set; }
    int CompletedOrderCount { get; set; }
    int TargetScore { get; }
    // Add other state properties as needed
}
