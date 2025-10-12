public class MockGameState : IGameState
{
    public int TotalScore { get; set; }
    public int CompletedOrderCount { get; set; }
    public int TargetScore { get; set; } = 500; // Example target
}
