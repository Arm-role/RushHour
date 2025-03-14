using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RushHour.FusionBits
{
    public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
    {
        [HideInInspector] public NetworkRunner runner;
        public NetworkObject PlayerPrefab;
        public string _playerName = "tester";
        public string _roomName = "1234";

        public delegate void OnPlayerJoin();
        public OnPlayerJoin onSandPlayersJoin { get; set; }

        public delegate void OnPlayersLeft(PlayerRef Players);
        public OnPlayersLeft onSandPlayersLeft { get; set; }

        public void SetName(TMP_InputField name)
        {
            _playerName = name.text;
        }
        public void setRoomName(TMP_InputField roomName)
        {
            _roomName = roomName.text;
        }
        public void JoinRoom()
        {
            SceneManager.LoadScene("Lobby");
            ConnectToRunner();
        }
        public void ConnectToRunner()
        {
            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }
            StartGame();
        }
        async void StartGame()
        {
            var result = await runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = _roomName,
                PlayerCount = 4,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });

            if (result.Ok)
            {
                Debug.Log("OK");
            }
            else
            {
                Debug.LogError($"Failed to start game: {result.ShutdownReason}");
            }
        }
        #region Network
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            onSandPlayersJoin?.Invoke();
        }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            onSandPlayersLeft?.Invoke(player);

            NetworkObject playerObject = runner.GetPlayerObject(player);

            if (playerObject != null)
            {
                runner.Despawn(playerObject);
            }
        }
        public void OnConnectedToServer(NetworkRunner runner)
        {
            NetworkObject playerObject = runner.Spawn(PlayerPrefab, Vector3.zero, Quaternion.identity, runner.LocalPlayer);
            runner.SetPlayerObject(runner.LocalPlayer, playerObject);

            if (playerObject.TryGetComponent<PlayerNetwork>(out PlayerNetwork player))
            {
                player.playerName = _playerName;
            }
        }
        #region UnUese
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data) { }
        #endregion
        #endregion
    }
}
