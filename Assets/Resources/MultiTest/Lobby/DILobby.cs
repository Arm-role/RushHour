using Fusion;
using System;
using System.Collections.Generic;

public class DILobby
{
    private static DILobby instance;
    public static DILobby Instance => instance ??= new DILobby();

    private Dictionary<PlayerRef, bool> _playerReady = new Dictionary<PlayerRef, bool>();
    private Dictionary<PlayerRef, PlayerLobbyData> _playerLobby = new Dictionary<PlayerRef, PlayerLobbyData>();
    private Action ReadyChanged;


    public void OnReadyChanged(PlayerRef player, bool isReady)
    {
        _playerReady[player] = isReady;
        ReadyChanged?.Invoke();
    }
    public void RemoveReady(PlayerRef player) => _playerReady.Remove(player);
    public Dictionary<PlayerRef, bool> GetReadies() { return _playerReady; }



    public void AddPlayerLobby(PlayerLobbyData playerLobby)
    {
        _playerLobby[playerLobby.player] = playerLobby;
    }
    public void RemovePlayerLobby(PlayerRef playerRef)
    {
        _playerLobby.Remove(playerRef);
    }

    public PlayerLobbyData GetPlayerLobbyData(PlayerRef playerRef) { return _playerLobby[playerRef]; }
    public Dictionary<PlayerRef, PlayerLobbyData> GetPlayerLobby() { return _playerLobby; }



    public void RegisterReady(Action ready)
    {
        ReadyChanged = ready;
    }
}
