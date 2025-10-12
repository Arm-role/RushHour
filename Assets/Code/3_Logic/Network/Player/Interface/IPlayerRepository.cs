using System.Collections.Generic;

public interface IPlayerRepository
{
    IPlayer GetPlayer(int id);
    IEnumerable<IPlayer> GetAllPlayers();
}
