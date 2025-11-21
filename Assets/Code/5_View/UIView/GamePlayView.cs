using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GamePlayView : MonoBehaviour, IGamePlayView
{
    [Header("UI Panels")]
    [SerializeField] private GameObject[] masterPanel;
    [SerializeField] private GameObject[] clientPanel;

    [Header("Set Position UI")]
    [SerializeField] private Button _lobbyButton;
    [SerializeField] private Button _exitButton;

    public event Action OnLobbyRoomPressed;
    public event Action OnLeaveRoomPressed;

    private GamePlayController _controller;

    private bool _initialIsCreateRoom;
    
    public void Start()
    {
        var player = PlayerRegistry.Instance.GetLocalPlayer();

        _initialIsCreateRoom = player.IsMaster;

        var gameState = FindAnyObjectByType<GameState>();
        var networkManager = FindObjectOfType<NetworkManager>();

        _controller = new GamePlayController(gameState, this, networkManager);


        SubscribeToUIEvents();
        ShowGameEndView();
    }

    private void OnDisable() 
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
                _lobbyButton.gameObject.SetActive(localPlayer.IsMaster);
            }
        }
    }
   
    private void SubscribeToUIEvents()
    {
        _lobbyButton.onClick.AddListener(() => OnLobbyRoomPressed?.Invoke());
        _exitButton.onClick.AddListener(() => OnLeaveRoomPressed?.Invoke());
    }
    private void UnsubscribeFromUIEvents()
    {
        _lobbyButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }
    public void ShowGameEndView()
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
        _controller?.Shutdown();
    }
}