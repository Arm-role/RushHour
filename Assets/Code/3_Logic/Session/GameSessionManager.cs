using Fusion;
using GameEvents;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSessionManager : MonoBehaviour
{
    private NetworkManager _networkManager;
    private PlayerSpawner _playerSpawner;

    private const string CONNECTION_LOST_SCENE_NAME = "ConnectionLost";

    private void Start()
    {
        var runnerGO = FindObjectOfType<NetworkRunner>().gameObject;
        _networkManager = runnerGO.GetComponent<NetworkManager>();
        _playerSpawner = runnerGO.GetComponent<PlayerSpawner>();

        if (_networkManager != null)
        {
            _networkManager.OnDisconnected += HandleOwnDisconnection;
        }

        if (_playerSpawner != null)
        {
            // This event fires when ANY player leaves the game
            // We need to implement this event in PlayerSpawner first
            // For now, let's assume PlayerSpawner has `public event Action<PlayerRef> OnPlayerLeftEvent;`
            // And it's invoked in PlayerLeft method.
            // Let's modify IPlayerLeft to handle it directly for now.
        }

        // We will handle player leaving logic inside a different component or service
        // that has access to PlayerRegistry.
    }

    private void OnDestroy()
    {
        // --- Unsubscribe from Events ---
        if (_networkManager != null)
        {
            _networkManager.OnDisconnected -= HandleOwnDisconnection;
        }
    }

    /// <summary>
    /// Called when our own client loses connection to the server for any reason.
    /// </summary>
    private void HandleOwnDisconnection(NetworkRunner runner)
    {
        Debug.Log("HandleOwnDisconnection: Our own connection was lost. Cleaning up.");
        EndSessionAndLoadConnectionLostScene(runner, "การเชื่อมต่อของคุณขัดข้อง");
    }

    public void HandleMasterClientLeft(NetworkRunner runner)
    {
        Debug.LogWarning("HandleMasterClientLeft: The Master Client has left the session. Ending game for all remaining players.");
        EndSessionAndLoadConnectionLostScene(runner, "โฮสต์ได้ออกจากการเชื่อมต่อ");
    }

    private void EndSessionAndLoadConnectionLostScene(NetworkRunner runner, string reason)
    {
        PlayerPrefs.SetString("DisconnectionReason", reason);
        PlayerPrefs.Save();

        if (runner != null && !runner.IsShutdown)
        {
            GameFlowState.Set(EGameFlow.GameNetworkShutdow);
            runner.Shutdown(); // We don't need to await this.
        }

        SceneManager.LoadScene(CONNECTION_LOST_SCENE_NAME);
    }
}