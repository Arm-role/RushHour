using UnityEngine;
using System.Collections.Generic;
using Fusion;
using System.Linq;

public class GamePlayController
{
    private GameState _gameState;
    private NetworkManager _networkManager;

    private IGamePlayView _view;

    public GamePlayController(GameState gameState, IGamePlayView view, NetworkManager networkManager)
    {
        _gameState = gameState;
        _networkManager = networkManager;

        _view = view;

        _view.OnLobbyRoomPressed += HandleLobbyRoomPressed;
        _view.OnLeaveRoomPressed += HandleLeaveRoomPressed;
    }

    private void HandleLobbyRoomPressed()
    {
        var localPlayer = PlayerRegistry.Instance.GetLocalPlayer();
        if (localPlayer == null || !localPlayer.IsMaster)
        {
            Debug.Log("Only the master can start the game.");
            return;
        }

        var playerNetworks = PlayerRegistry.Instance.GetAllPlayers().OrderBy(p => p.PlayerRef.PlayerId).ToList();
        _networkManager.StartGameScene(SceneIndex: 2);
    }
    private void HandleLeaveRoomPressed()
    {
        _networkManager.StartGameScene(SceneIndex: 0);
        _networkManager.LeaveRoom();
    }
    public void Shutdown()
    {
        _view.OnLobbyRoomPressed -= HandleLobbyRoomPressed;
        _view.OnLeaveRoomPressed -= HandleLeaveRoomPressed;
    }
}