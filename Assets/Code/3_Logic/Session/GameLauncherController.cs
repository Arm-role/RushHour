using UnityEngine;
using SessionEvent;
using System.Collections.Generic;
using Fusion;
using System.Linq;

public class GameLauncherController
{
    private bool _isCreateRoom;

    private int _keyCodeLength = 5;
    private List<int> _currentCode = new List<int>();
    private int _keyButtonCount;
    private int codeIndex = 0;

    private IGameLauncherView _view;
    private NetworkManager _networkManager;

    private List<SessionInfo> ActiveSessionInfos = new List<SessionInfo>();

    public GameLauncherController(IGameLauncherView view, NetworkManager networkManager, int keyButtonCount, bool isCreateRoom)
    {
        _view = view;
        _networkManager = networkManager;

        _keyButtonCount = keyButtonCount;
        _isCreateRoom = isCreateRoom;

        // Subscribe to events from the View
        _view.OnKeyPressed += HandleKeyPressed;
        _view.OnResetPressed += HandleResetPressed;
        _view.OnRollPressed += HandleRollPressed;
        _view.OnCreateRoomPressed += HandleCreateRoomPressed;
        _view.OnJoinRoomPressed += HandleJoinRoomPressed;
        _view.OnStartGamePressed += HandleStartGamePressed;
        _view.OnLeaveRoomPressed += HandleLeaveRoomPressed;

        // Subscribe to events from the NetworkManager
        _networkManager.OnSessionListUpdatedEvent += HandleSessionListUpdated;
        _networkManager.OnPlayerJoinedEvent += HandlePlayerJoined;
        _networkManager.OnPlayerLeavedEvent += HandlePlayerLeaved;

    }
    private string GetRoomNameFromCode()
    {
        return string.Join("", _currentCode);
    }
    public void HandleKeyPressed(int key)
    {
        Debug.Log($"key {key}");

        if (codeIndex < _keyCodeLength)
        {
            _currentCode.Add(key);

            Debug.Log($"menuCodePos {codeIndex}");

            EventManager.Invoke(new CodeSession(codeIndex, key));
            codeIndex++;
        }

        if (_currentCode.Count == _keyCodeLength)
        {
            int i = 0;
            string rommName = string.Empty;
            foreach (var code in _currentCode)
            {
                Debug.Log($"code{i} : {code}");
                rommName = rommName + code;
                i++;
            }

            Debug.Log($"Name{rommName}");
            EventManager.Invoke(new MenuCodeSession(_currentCode.ToArray()));
        }
    }
    public void HandleResetPressed()
    {
        _currentCode.Clear();
        codeIndex = 0;

        Debug.Log($"Reset");
        EventManager.Invoke(new ResetCode());
    }
    private void HandleRollPressed()
    {
        if (codeIndex > _keyCodeLength - 1) return;

        for (int i = 0; i < _keyCodeLength; i++)
        {
            HandleKeyPressed(Random.Range(0, _keyButtonCount));
        }

        Debug.Log($"Roll");
        EventManager.Invoke(new RollCode());
    }
    private void HandleCreateRoomPressed()
    {
        _networkManager.CreateOrJoinRoom(GetRoomNameFromCode(), 6, true);
        _view.ShowLobbyView();
    }
    private void HandleJoinRoomPressed()
    {
        if (HaveSelectSessionUpdate(GetRoomNameFromCode()))
        {
            _networkManager.CreateOrJoinRoom(GetRoomNameFromCode(), 6, false);
            _view.ShowLobbyView();
        }
        else
        {
            Debug.Log("Don't Have Room");
        }
    }
    private void HandleStartGamePressed()
    {
        _networkManager.StartGameScene(SceneIndex: 3);
    }
    private void HandleLeaveRoomPressed()
    {
        _networkManager.LeaveRoom();
    }
    private void HandleSessionListUpdated(List<SessionInfo> sessionList)
    {
        string roomName = GetRoomNameFromCode();

        ActiveSessionInfos = sessionList;

        foreach (var session in sessionList)
        {
            if (session.Name == roomName)
            {
                _view.UpdatePlayerCount(session.PlayerCount, session.MaxPlayers);
                return;
            }
        }

        if (_isCreateRoom)
        {
            AutoCreateRoom();
        }
    }
    private bool HaveSelectSessionUpdate(string roomName)
    {
        if (ActiveSessionInfos.Count < 1) return false;

        foreach (var session in ActiveSessionInfos)
        {
            if (session.Name == roomName)
            {
                return true;
            }
        }

        return false;
    }
    private void HandlePlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        _view.UpdatePlayerCount(runner.ActivePlayers.Count(), runner.SessionInfo.MaxPlayers);
    }
    private void HandlePlayerLeaved(NetworkRunner runner, PlayerRef player)
    {
        _view.UpdatePlayerCount(runner.ActivePlayers.Count(), runner.SessionInfo.MaxPlayers);
    }
    private void AutoCreateRoom()
    {
        HandleRollPressed();
        HandleCreateRoomPressed();
    }
    public void Shutdown()
    {
        // Unsubscribe all events to prevent memory leaks
    }
}
