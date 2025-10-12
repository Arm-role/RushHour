using Fusion;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerService : MonoBehaviour
{
    public static PlayerService Instance { get; private set; }

    // --- State Properties ---
    private GameState _gameState;
    private PlayerNetwork _localPlayer;

    public PlayerNetwork LeftNeighbor { get; private set; }
    public PlayerNetwork RightNeighbor { get; private set; }
    public bool IsInitialized { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Make it persistent across scenes
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"PlayerService detected scene loaded: {scene.name}");
        if (scene.name == "Game") 
        {
            Initialize();
        }
        else
        {
            IsInitialized = false;
            LeftNeighbor = null;
            RightNeighbor = null;
        }
    }

    private void Initialize()
    {
        _gameState = FindObjectOfType<GameState>();
        if (_gameState == null)
        {
            Debug.LogError("PlayerService could not find GameState in the game scene!");
            return;
        }

        UpdateNeighbors();
        IsInitialized = true;
    }

    private void UpdateNeighbors()
    {
        if (_gameState == null)
        {
            _gameState = FindObjectOfType<GameState>();
            if (_gameState == null) return;
        }

        if (_gameState.SeatingOrder.Count <= 1)
        {
            LeftNeighbor = RightNeighbor = null;
            return;
        }

        if (_localPlayer == null)
            _localPlayer = PlayerRegistry.Instance.GetLocalPlayer();

        if (_localPlayer == null) return;

        List<PlayerRef> seatingOrder = _gameState.SeatingOrder.ToList();
        int myIndex = seatingOrder.IndexOf(_localPlayer.PlayerRef);

        if (myIndex == -1) return;

        int prevIndex = (myIndex - 1 + seatingOrder.Count) % seatingOrder.Count;
        int nextIndex = (myIndex + 1) % seatingOrder.Count;

        LeftNeighbor = PlayerRegistry.Instance.GetPlayer(seatingOrder[prevIndex]);
        RightNeighbor = PlayerRegistry.Instance.GetPlayer(seatingOrder[nextIndex]);

        Debug.Log($"My Neighbors Updated! Left: {LeftNeighbor?.PlayerRef.PlayerId}, Right: {RightNeighbor?.PlayerRef.PlayerId}");
    }
}