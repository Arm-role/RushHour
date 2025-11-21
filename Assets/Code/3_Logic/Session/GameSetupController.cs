using UnityEngine;
using SessionEvent;
using System.Collections.Generic;
using Fusion;
using System.Linq;
using System;

public class GameSetupController
{
    private int _playerCount;
    private List<int> _selectedKeys = new List<int>();

    private GameState _gameState;

    private IGameSetupView _view;
    private NetworkManager _networkManager;

    public event Action<int> OnSelectedKey;
    public event Action<int> OnRejectedKey;

    public event Action<List<int>> OnRollKey;
    public event Action<List<int>> OnResetKey;

   

    public GameSetupController(GameState gameState, IGameSetupView view, NetworkManager networkManager, GameSetupSpriteView _spriteView,
                               NetworkedPlateController _networkedPlateController, int playerCount)
    {
        _gameState = gameState;

        _view = view;
        _networkManager = networkManager;

        _playerCount = playerCount;

        // Subscribe to events from the View
        _view.OnKeyPressed += HandleKeyPressed;
        _view.OnResetPressed += HandleResetPressed;
        _view.OnRollPressed += HandleRollPressed;
        _view.OnStartGamePressed += HandleStartGamePressed;
        _view.OnLeaveRoomPressed += HandleLeaveRoomPressed;

        OnSelectedKey += _spriteView.SelectedKey;
        OnRejectedKey += _spriteView.RejectedKey;
        OnResetKey += _spriteView.ResetKey;
        OnRollKey += _spriteView.RollKey;

        OnSelectedKey += _networkedPlateController.SelectedKey;
        OnRejectedKey += _networkedPlateController.RejectedKey;
        OnResetKey += _networkedPlateController.ResetKey;
        OnRollKey += _networkedPlateController.RollKey;
    }
    public void HandleKeyPressed(int key)
    {
        if (_selectedKeys.Contains(key))
        {
            _selectedKeys.Remove(key);
            OnRejectedKey?.Invoke(key);
        }
        else
        {
            _selectedKeys.Add(key);
            OnSelectedKey?.Invoke(key);
        }
    }
    public void HandleResetPressed()
    {
        var keysToReset = new List<int>(_selectedKeys);

        _selectedKeys.Clear();

        OnResetKey?.Invoke(keysToReset);
    }
    private void HandleRollPressed()
    {
        HandleResetPressed();

        List<int> allPossibleKeys = Enumerable.Range(0, _playerCount).ToList();

        var randomKeys = allPossibleKeys.OrderBy(k => Guid.NewGuid()).Take(_playerCount);

        _selectedKeys = randomKeys.ToList();

        OnRollKey?.Invoke(_selectedKeys);
    }

    private void HandleStartGamePressed()
    {
        var localPlayer = PlayerRegistry.Instance.GetLocalPlayer();
        if (localPlayer == null || !localPlayer.IsMaster)
        {
            Debug.Log("Only the master can start the game.");
            return;
        }

        if (_selectedKeys.Count != _playerCount)
        {
            Debug.LogError($"Cannot start game. {_selectedKeys.Count} keys selected, but {_playerCount} players are in the room.");
            return;
        }

        var playerNetworks = PlayerRegistry.Instance.GetAllPlayers().OrderBy(p => p.PlayerRef.PlayerId).ToList();
        var finalPlayerOrder = new List<PlayerRef>();

        for (int i = 0; i < _selectedKeys.Count; i++)
        {
            finalPlayerOrder.Add(playerNetworks[_selectedKeys[i]].PlayerRef);
        }

        _gameState.SeatingOrder.Clear();
        foreach (var playerRef in finalPlayerOrder)
        {
            _gameState.SeatingOrder.Add(playerRef);
        }

        _networkManager.StartGameScene(SceneIndex: 3);
    }
    private void HandleLeaveRoomPressed()
    {
        _networkManager.LeaveRoom();
    }
    public void Shutdown()
    {
        // Unsubscribe all events to prevent memory leaks
    }

  
}
