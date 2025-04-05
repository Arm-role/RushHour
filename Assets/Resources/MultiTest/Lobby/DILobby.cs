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
    public Dictionary<PlayerRef, bool> GetReadies() { return _playerReady; }



    public void AddPlayerLobby(PlayerLobbyData playerLobby)
    {
        _playerLobby[playerLobby.player] = playerLobby;
    }
    public Dictionary<PlayerRef, PlayerLobbyData> GetPlayerLobby() { return _playerLobby; }



    public void RegisterReady(Action ready)
    {
        ReadyChanged = ready;
    }
}
