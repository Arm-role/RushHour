using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameSetupView : MonoBehaviour, IGameSetupView
{
    [Header("UI Panels")]
    [SerializeField] private GameObject[] masterPanel;
    [SerializeField] private GameObject[] clientPanel;

    [Header("Set Position UI")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _rollButton;
    [SerializeField] private Button _resetButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button[] _keyButtons;

    public event Action<int> OnKeyPressed;
    public event Action OnResetPressed;
    public event Action OnRollPressed;
    public event Action OnStartGamePressed;
    public event Action OnLeaveRoomPressed;

    private GameSetupController _controller;
    private GameSetupSpriteView _spriteView;

    private NetworkedPlateController _networkedPlateController;

    private bool _initialIsCreateRoom;

    public void Start()
    {
        var player = PlayerRegistry.Instance.GetLocalPlayer();
        int playerCount = PlayerRegistry.Instance.GetAllPlayers().Count();
        _initialIsCreateRoom = player.IsMaster;

        var networkManager = FindObjectOfType<NetworkManager>();

        _spriteView = GetComponent<GameSetupSpriteView>();
        _networkedPlateController = FindObjectOfType<NetworkedPlateController>();

        var gameState = FindObjectOfType<GameState>();
        _controller = new GameSetupController(gameState, this, networkManager, _spriteView, _networkedPlateController, playerCount);

        _spriteView.SetIconImage(PlayerRegistry.Instance.GetPlayerIndex(player));

        SetActiveKeyByPlayerCount(playerCount);
        SubscribeToUIEvents();
        ShowLobbyView();
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
    private void SetActiveKeyByPlayerCount(int count)
    {
        if (_keyButtons.Length < count) return;

        for (int i = 0; i < _keyButtons.Length; i++)
        {
            GameObject keyButton = _keyButtons[i].gameObject;

            if (i < count)
            {
                keyButton.SetActive(true);
            }
            else
            {
                keyButton.SetActive(false);
            }
        }
    }
    private void SubscribeToUIEvents()
    {
        _resetButton.onClick.AddListener(() => OnResetPressed?.Invoke());
        _rollButton.onClick.AddListener(() => OnRollPressed?.Invoke());
        _startButton.onClick.AddListener(() => OnStartGamePressed?.Invoke());
        _backButton.onClick.AddListener(() => OnLeaveRoomPressed?.Invoke());

        _controller.OnSelectedKey += _spriteView.SelectedKey;
        _controller.OnRejectedKey += _spriteView.RejectedKey;

        for (int i = 0; i < _keyButtons.Length; i++)
        {
            int index = i;
            _keyButtons[i].onClick.AddListener(() => OnKeyPressed?.Invoke(index));
        }
    }
    private void UnsubscribeFromUIEvents()
    {
        _resetButton.onClick.RemoveAllListeners();
        _rollButton.onClick.RemoveAllListeners();
        _startButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();

        _controller.OnSelectedKey -= _spriteView.SelectedKey;
        _controller.OnRejectedKey -= _spriteView.RejectedKey;

        foreach (var btn in _keyButtons)
        {
            btn.onClick.RemoveAllListeners();
        }
    }
    public void ShowLobbyView()
    {
        if (_initialIsCreateRoom)
        {
            SetPanelVisibility(masterPanel, true);
            SetPanelVisibility(clientPanel, false);
        }
        else
        {
            SetPanelVisibility(masterPanel, false);
            SetPanelVisibility(clientPanel, true);
        }
    }

    private void SetPanelVisibility(GameObject[] panels, bool visible)
    {
        foreach (var panel in panels)
        {
            panel.SetActive(visible);
        }
    }
    private void OnDestroy()
    {
        _controller?.Shutdown(); // สั่งให้ Controller ยกเลิกการ Subscribe ทั้งหมด
    }

}
