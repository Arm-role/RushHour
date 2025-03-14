using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

public class HostMigrationManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [Networked] private PlayerRef VirtualHost { get; set; } // ตัวแทนผู้เล่นที่เป็น Host เสมือน

    private void Start()
    {
        // เพิ่ม Callback ให้กับ NetworkRunner
        NetworkRunner runner = FindObjectOfType<NetworkRunner>();
        if (runner != null)
        {
            runner.AddCallbacks(this);

            // กำหนด Host เริ่มต้น
            if (runner.IsServer && VirtualHost == default)
            {
                VirtualHost = runner.LocalPlayer;
                Debug.Log($"Initial Host is {VirtualHost}");
            }
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player {player} has left the game.");

        // หากผู้เล่นที่ออกคือ Host เสมือน ให้เลือก Host คนใหม่
        if (player == VirtualHost)
        {
            Debug.Log("Host has disconnected. Selecting a new host...");
            SelectNewHost(runner);
        }
    }

    private void SelectNewHost(NetworkRunner runner)
    {
        foreach (PlayerRef player in runner.ActivePlayers)
        {
            if (player != VirtualHost) // ไม่เลือกคนเดิมที่หลุด
            {
                VirtualHost = player; // กำหนด Host ใหม่
                Debug.Log($"New Host is {VirtualHost}");
                break;
            }
        }
    }
    #region UnUsed
    // ไม่จำเป็นต้อง Override ฟังก์ชันอื่นที่ไม่เกี่ยวข้อง
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    #endregion
}
