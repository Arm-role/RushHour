using Fusion;
using System;
using UnityEngine;
using Fusion.Sockets;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    public event Action OnJoinedLobbySuccess;
    public event Action<List<SessionInfo>> OnSessionListUpdatedEvent;
    public event Action<NetworkRunner, PlayerRef> OnPlayerJoinedEvent;
    public event Action<NetworkRunner, PlayerRef> OnPlayerLeavedEvent;

    public event Action<NetworkRunner> OnDisconnected;

    private void Awake()
    {
        var managers = FindObjectsOfType<NetworkManager>();
        if (managers.Length > 1) { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
    }
    public async void ConnectToLobby()
    {
        if (_runner != null) return;

        _runner = GetComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        _runner.AddCallbacks(this);

        var result = await _runner.JoinSessionLobby(SessionLobby.Shared);
        if (result.Ok)
        {
            OnJoinedLobbySuccess?.Invoke();
        }
        else
        {
            Debug.LogError($"Failed to join lobby: {result.ShutdownReason}");
        }
    }
    public void OnChangeScene()
    {
        if (_runner == null || !_runner.IsSharedModeMasterClient)
        {
            Debug.LogWarning("Only the Master Client can start the game.");
            return;
        }

        Debug.Log("Master Client is loading the game scene for everyone...");

        _runner.LoadScene(SceneRef.FromIndex(3));
    }
    public async void CreateOrJoinRoom(string sessionName, int playerCount, bool isMaster)
    {
        if (_runner == null)
        {
            Debug.LogError("Not connected to lobby. Cannot start a room.");
            return;
        }

        var startGameArgs = new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = sessionName,
            PlayerCount = playerCount
        };

        var result = await _runner.StartGame(startGameArgs);

        if (result.Ok)
        {
        }
        else
        {
            Debug.LogError($"Failed to start/join room: {result.ShutdownReason}");
        }
    }

    public void StartGameScene(int SceneIndex)
    {
        if (_runner == null || !_runner.IsSharedModeMasterClient) return;
        _runner.LoadScene(SceneRef.FromIndex(SceneIndex));
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        OnSessionListUpdatedEvent?.Invoke(sessionList);
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        OnPlayerJoinedEvent?.Invoke(runner, player);
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        OnPlayerLeavedEvent?.Invoke(runner, player);
    }

    public async void LeaveRoom()
    {
        if (_runner == null)
        {
            Debug.LogWarning("Runner is not active or already null. Cannot leave room.");
            return;
        }

        Debug.Log("Leaving room and shutting down the Network Runner...");

        await _runner.Shutdown(true);
        _runner = null;

        Debug.Log("Network Manager is being destroyed to reset network state.");
        Destroy(gameObject);
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.LogWarning($"OnDisconnectedFromServer triggered for runner {runner.GetInstanceID()}");
        OnDisconnected?.Invoke(runner);
    }

    #region Unused Callbacks
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    #endregion
}
