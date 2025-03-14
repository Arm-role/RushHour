using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerManager
{
    private static PlayerManager _instance;
    public static PlayerManager Instance => _instance ??= new PlayerManager();


    private Dictionary<PlayerRef, PlayerNetwork> players;
    public Dictionary<PlayerRef, PlayerNetwork> Players => players ??= DIPlayerContain.Instance.GetPlayers();

    public delegate void SentPlayerNetwork(PlayerNetwork player);
    public event SentPlayerNetwork OnSentPlayerNetwork;

    public PlayerManager()
    {
        EvenManager.End += OnEnd;

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
        int rand = Random.Range(0, Players.Count);
        List<PlayerNetwork> allplayer = Players.Values.ToList();

        return allplayer[rand];
    }
    public List<PlayerNetwork> GetPlayerList()
    {
        return Players.Values.ToList();
    }
    public void OnEnd(bool isEnd)
    {
        if (isEnd)
        {
            foreach (var player in Players.Values)
            {
                OnSentPlayerNetwork?.Invoke(player);
            }
        }
    }
}
