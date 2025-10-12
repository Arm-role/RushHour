public class MockPlayer : IPlayer
{
    public int Id { get; }
    public int Score { get; set; }
    public MockPlayer(int id) { Id = id; }
}
