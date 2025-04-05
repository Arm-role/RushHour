using Fusion;
using System.Collections.Generic;
using System.Linq;
public class PlayerManager
{
    private static PlayerManager _instance;
    public static PlayerManager Instance => _instance ??= new PlayerManager();


    private Dictionary<PlayerRef, PlayerNetwork> players;
    public Dictionary<PlayerRef, PlayerNetwork> Players => players ??= DIPlayerContain.Instance.GetPlayers();

    public PlayerManager()
    {
        GameEvents.Instance.OnGameState.Subscribe(OnGameRun);

        DIPlayerContain.Instance.RegisterAddPlayerNetwork(OnPlayerAdd);
        DIPlayerContain.Instance.RegisterRemovePlayerNetwork(OnPlayerRemove);
    }
    public void OnPlayerAdd(PlayerNetwork player)
    {
        Players.Add(player.playerRef, player);
    }
    public void OnPlayerRemove(PlayerRef player)
    {
        Players.Remove(player);
    }
    public PlayerNetwork GetNearPlayer(int index)
    {
        return GetListNearPlayer(DIPlayerContain.Instance.LocalPlayer)[index];
    }
    private List<PlayerNetwork> GetListNearPlayer(PlayerNetwork OwnNetwork)
    {
        List<PlayerNetwork> allplayer = Players.Values.ToList();

        if (allplayer.Count <= 1) return new List<PlayerNetwork> { OwnNetwork, OwnNetwork };

        int index = allplayer.IndexOf(OwnNetwork);
        int previousIndex = (index - 1 + allplayer.Count) % allplayer.Count;
        int nextIndex = (index + 1) % allplayer.Count;

        return new List<PlayerNetwork> { allplayer[previousIndex], allplayer[nextIndex] };
    }

    public PlayerNetwork RandomPlayer()
    {
        int rand = UnityEngine.Random.Range(0, Players.Count);
        List<PlayerNetwork> allplayer = Players.Values.ToList();

        return allplayer[rand];
    }
    public List<PlayerNetwork> GetPlayerList()
    {
        return Players.Values.ToList();
    }
    public void OnGameRun(EGameState gameState)
    {
        if (gameState == EGameState.End)
        {
            foreach (var player in Players.Values)
            {
                PlayerEvents.Instance.OnSentPlayerNetwork.Invoke(player);
            }
        }
    }
}
