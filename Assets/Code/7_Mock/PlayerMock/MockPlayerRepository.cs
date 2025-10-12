using System.Collections.Generic;
public class MockPlayerRepository : IPlayerRepository
{
    private Dictionary<int, IPlayer> _players = new Dictionary<int, IPlayer>();

    public void AddPlayer(IPlayer player) => _players.Add(player.Id, player);
    public IPlayer GetPlayer(int id) => _players.TryGetValue(id, out var p) ? p : null;
    public IEnumerable<IPlayer> GetAllPlayers() => _players.Values;
}
