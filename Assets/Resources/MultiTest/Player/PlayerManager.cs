using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerManager
{
    private static PlayerManager _instance;
    public static PlayerManager Instance => _instance ??= new PlayerManager();


    public IReadOnlyDictionary<PlayerRef, PlayerNetwork> Player => DIPlayerContain.Instance.GetAllPlayers();
    public PlayerManager()
    {
        GameEvents.Instance.OnGameState.Subscribe(OnGameStateChanged);
    }

    public PlayerNetwork GetNearPlayer(int index)
    {
        var localPlayer = DIPlayerContain.Instance.LocalPlayer;
        var NearbyPlayers = GetNearbyPlayers(localPlayer);

        return NearbyPlayers.ElementAtOrDefault(index);
    }

    private List<PlayerNetwork> GetNearbyPlayers(PlayerNetwork ownPlayer)
    {
        var playerList = Player.Values.ToList();

        if (playerList.Count <= 1) return new List<PlayerNetwork> { ownPlayer, ownPlayer };

        int index = playerList.IndexOf(ownPlayer);
        int priv = (index - 1 + playerList.Count) % playerList.Count;
        int next = (index + 1) % playerList.Count;

        Debug.Log($"{playerList[priv].PlayerName} : {playerList[next].PlayerName}");

        return new List<PlayerNetwork> { playerList[priv], playerList[next] };
    }
    public void OnGameStateChanged(EGameState state)
    {
        if (state == EGameState.End)
        {
            foreach (var player in Player.Values)
            {
                PlayerEvents.Instance.OnSentPlayerScore?.Invoke((player.PlayerName, player.GetScore()));
            }


            var playerOnKeeper = PlayerEvents.Instance.OnKeepPlayerScore.GetParamiter();

            foreach (var player in playerOnKeeper)
            {
                PlayerEvents.Instance.OnSentPlayerScore?.Invoke((player.Key, player.Value));
            }
        }
    }
}
