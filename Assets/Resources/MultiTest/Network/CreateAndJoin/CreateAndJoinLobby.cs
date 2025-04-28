using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;
using UnityEngine.SceneManagement;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

public class CreateAndJoinLobby : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private TMP_InputField roomNameInput;  // Input สำหรับชื่อห้อง
    [SerializeField] private Button button;  // ปุ่มสร้างห้อง
    [SerializeField] private GameObject fusionObject;    // ปุ่มเข้าร่วมห้อง

    private NetworkRunner _networkRunner;
    private List<SessionInfo> availableSessions = new List<SessionInfo>(); // เก็บรายการห้องที่มีอยู่


    private void Start()
    {
        StartRunner();

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Create")
        {
            button.onClick.AddListener(OnCreateRoomClicked);
        }
        else if (currentScene == "Join")
        {
            button.onClick.AddListener(OnJoinRoomClicked);
        }
    }

    private async void StartRunner()
    {
        if (_networkRunner == null)
        {
            _networkRunner = fusionObject.AddComponent<NetworkRunner>();
            _networkRunner.AddCallbacks(this);
            DontDestroyOnLoad(fusionObject);
        }
        
        if (_networkRunner.IsConnectedToServer && _networkRunner.LobbyInfo != null)
        {
            return;
        }
        await _networkRunner.JoinSessionLobby(SessionLobby.Shared);
    }
    private async void OnCreateRoomClicked()
    {
        if (!string.IsNullOrEmpty(roomNameInput.text))
        {
            var result = await _networkRunner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Shared,
                SessionName = roomNameInput.text,
                PlayerCount = 4,
                IsVisible = true,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            });

            if (result.Ok)
            {
                Debug.Log("Room created successfully!");
                OnEnterRoom();
            }
            else
            {
                Debug.LogError($"Failed to create room: {result.ShutdownReason}");
            }
        }
    }
    private async void OnJoinRoomClicked()
    {
        if (!string.IsNullOrEmpty(roomNameInput.text))
        {
            // ตรวจสอบว่าห้องมีอยู่ใน availableSessions หรือไม่
            bool roomExists = availableSessions.Exists(session => session.Name == roomNameInput.text);

            if (roomExists)
            {
                // ถ้าห้องมีอยู่ ให้พยายามเข้าร่วมห้อง
                var result = await _networkRunner.StartGame(new StartGameArgs
                {
                    GameMode = GameMode.Shared,               // Shared mode
                    SessionName = roomNameInput.text,    // ชื่อห้อง
                    IsVisible = true,
                    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
                });

                if (result.Ok)
                {
                    Debug.Log("Joined room successfully!");
                    OnEnterRoom();
                }
                else
                {
                    Debug.LogError($"Failed to join room: {result.ShutdownReason}");
                }
            }
            else
            {
                Debug.LogWarning("The room you are trying to join does not exist!");
            }
        }
        else
        {
            Debug.LogWarning("Room name cannot be empty!");
        }
    }

    private void OnEnterRoom()
    {
        //DIPlayerContain.Instance.SetNetworkRunner(_networkRunner);
        SceneController.Instance.LoadScene("Lobby");
        GameEvents.Instance.OnGameScene?.Invoke(EGameScene.Lobby);
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log(player.ToString());

        if (player == runner.LocalPlayer)
        {
            runner.Spawn(PlayerPrefab, Vector2.zero, Quaternion.identity, player);
        }
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        availableSessions = sessionList;
        Debug.Log($"📡 อัพเดท Session List: พบ {sessionList.Count} ห้อง");

        string sessionInfo = $"🛎️ จำนวนห้องที่มีอยู่: {availableSessions.Count}\n";

        foreach (var session in availableSessions)
        {
            sessionInfo += $"🔹 ห้อง: {session.Name} | ผู้เล่น: {session.PlayerCount}/{session.MaxPlayers}\n";
        }
        Debug.Log(sessionInfo);
    }

    #region UnUsed
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
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
