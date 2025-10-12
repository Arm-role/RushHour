using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameLauncherView : MonoBehaviour, IGameLauncherView
{
    [Header("Network")]
    [SerializeField] private GameObject _networkManagerPrefab;

    [Header("UI Panels")]
    [SerializeField] private GameObject[] _mainMenuCreatePanels;
    [SerializeField] private GameObject[] _mainMenuJoinPanels;
    [SerializeField] private GameObject[] _lobbyCreatePanels;
    [SerializeField] private GameObject[] _lobbyJoinPanels;

    [Header("Main Menu UI")]
    [SerializeField] private Button _createButton;
    [SerializeField] private Button _joinButton;
    [SerializeField] private Button _resetButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button[] _keyButtons;
    // ... UI อื่นๆ สำหรับแสดงโค้ด

    [Header("Lobby UI")]
    [SerializeField] private TextMeshProUGUI _playerCountText;
    [SerializeField] private Button _startButton;

    public event Action<int> OnKeyPressed;
    public event Action OnResetPressed;
    public event Action OnRollPressed;
    public event Action OnCreateRoomPressed;
    public event Action OnJoinRoomPressed;
    public event Action OnStartGamePressed;
    public event Action OnLeaveRoomPressed;

    private GameLauncherController _controller;

    private bool _initialIsCreateRoom;

    private void Start()
    {
        _initialIsCreateRoom = UIManager.Instance.IsCreatingRoom;

        var networkManager = FindObjectOfType<NetworkManager>();

        if (networkManager == null)
        {
            networkManager = Instantiate(_networkManagerPrefab).GetComponent<NetworkManager>();
        }

        _controller = new GameLauncherController(this, networkManager, _keyButtons.Length, _initialIsCreateRoom);

        networkManager.ConnectToLobby();
        SubscribeToUIEvents();

        ShowMainMenuView();
    }
    private void OnDisable() // Counterpart to OnEnable
    {
        UnsubscribeFromUIEvents();
        _controller?.Shutdown();
    }

    private void Update()
    {
        if (PlayerRegistry.Instance != null && PlayerRegistry.Instance.GetAllPlayers().Any())
        {
            var localPlayer = PlayerRegistry.Instance.GetLocalPlayer();
            if (localPlayer != null)
            {
                _startButton.gameObject.SetActive(localPlayer.IsMaster);
            }
        }
    }
    private void SubscribeToUIEvents()
    {
        _createButton.onClick.AddListener(() => OnCreateRoomPressed?.Invoke());
        _joinButton.onClick.AddListener(() => OnJoinRoomPressed?.Invoke());
        _resetButton.onClick.AddListener(() => OnResetPressed?.Invoke());
        _startButton.onClick.AddListener(() => OnStartGamePressed?.Invoke());
        _backButton.onClick.AddListener(() => OnLeaveRoomPressed?.Invoke());

        for (int i = 0; i < _keyButtons.Length; i++)
        {
            int index = i;
            _keyButtons[i].onClick.AddListener(() => OnKeyPressed?.Invoke(index));
        }
    }
    private void UnsubscribeFromUIEvents()
    {
        _createButton.onClick.RemoveAllListeners();
        _joinButton.onClick.RemoveAllListeners();
        _resetButton.onClick.RemoveAllListeners();
        _startButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();

        foreach (var btn in _keyButtons)
        {
            btn.onClick.RemoveAllListeners();
        }
    }
    public void ShowMainMenuView()
    {
        if (_initialIsCreateRoom)
        {
            SetPanelVisibility(_mainMenuCreatePanels, true);
            SetPanelVisibility(_lobbyCreatePanels, false);
        }
        else
        {
            SetPanelVisibility(_mainMenuJoinPanels, true);
            SetPanelVisibility(_lobbyJoinPanels, false);
        }
    }

    public void ShowLobbyView()
    {
        if (_initialIsCreateRoom)
        {
            SetPanelVisibility(_mainMenuCreatePanels, false);
            SetPanelVisibility(_lobbyCreatePanels, true);
        }
        else
        {
            SetPanelVisibility(_mainMenuJoinPanels, false);
            SetPanelVisibility(_lobbyJoinPanels, true);
        }
    }

    // Helper method to reduce code duplication
    private void SetPanelVisibility(GameObject[] panels, bool visible)
    {
        foreach (var panel in panels)
        {
            panel.SetActive(visible);
        }
    }
    public void UpdatePlayerCount(int current, int max)
    {
        _playerCountText.text = $"{current}/{max}";
    }

    public void UpdateCodeDisplay(int index, int value)
    {
        // ... Logic แสดงผลโค้ดทีละตัว ...
    }

    public void ResetCodeDisplay()
    {
        // ... Logic ล้างการแสดงผลโค้ด ...
    }

    public void UpdateAllCodeDisplay(int[] code)
    {
        // ... Logic แสดงผลโค้ดทั้งหมด (หลังกด Roll) ...
    }

    private void OnDestroy()
    {
        _controller?.Shutdown(); // สั่งให้ Controller ยกเลิกการ Subscribe ทั้งหมด
    }
}
