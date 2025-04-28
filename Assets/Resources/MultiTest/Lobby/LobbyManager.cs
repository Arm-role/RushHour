using Fusion;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private int MaxTime;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button cancelButton;

    private LobbyFactory factory;
    private ReadySystem readySystem;
    private TimerSystem timerSystem;
    private Coroutine timeCorountine;

    private bool Ready = true;

    private Dictionary<PlayerRef, PlayerNetwork> players;

    private void Start()
    {
        textMesh.text = MaxTime.ToString();

        factory = new LobbyFactory(playerPrefab, parent);
        readySystem = new ReadySystem();
        timerSystem = new TimerSystem();

        var containPlayer = DIPlayerContain.Instance.GetAllPlayers();
        players = new Dictionary<PlayerRef, PlayerNetwork>(containPlayer);

        DIPlayerContain.Instance.OnPlayerAdd += GetPlayer;
        DIPlayerContain.Instance.OnPlayerRemove += RemovePlayer;

        DILobby.Instance.RegisterReady(OnReadyChanged);
        PlayerEvents.Instance.OnReady.Invoke(false);

        CreatePlayerFirst();
    }



    public void CreatePlayerFirst()
    {
        foreach (var player in players.Values)
        {
            GetPlayer(player);
        }
    }
    public void GetPlayer(PlayerNetwork playerNetwork)
    {
        var playerLobby = factory.CreatePlayerLobby(playerNetwork);

        DILobby.Instance.AddPlayerLobby(playerLobby);
        DILobby.Instance.OnReadyChanged(playerNetwork.PlayerRef, playerNetwork.IsReady);

        SetupPlayer(playerNetwork);
    }   
    public void RemovePlayer(PlayerRef playerRef)
    {
        var player = DILobby.Instance.GetPlayerLobbyData(playerRef);

        DILobby.Instance.RemovePlayerLobby(playerRef);
        DILobby.Instance.RemoveReady(playerRef);

        Destroy(player.Object);
    }

    private void SetupPlayer(PlayerNetwork playerNetwork)
    {
        if (playerNetwork.isLocalPlayer)
        {
            readyButton.onClick.AddListener(() => playerNetwork.SetReady(true));
            cancelButton.onClick.AddListener(() => playerNetwork.SetReady(false));
        }
    }


    public void OnReadyChanged()
    {
        var playerReady = DILobby.Instance.GetReadies();
        var playerLobby = DILobby.Instance.GetPlayerLobby();

        if (readySystem.Initialize(playerReady, playerLobby))
        {
            timeCorountine = StartCoroutine(timerSystem.TimeDelayCoroutine(
                textMesh, MaxTime, 1, 2, OnNotifyUnCancelReady, OnChangeScene
                ));
        }
        else if (Ready && timeCorountine != null)
        {
            StopCoroutine(timeCorountine);
            textMesh.text = MaxTime.ToString();
        }
    }
    public void OnNotifyUnCancelReady()
    {
        Debug.Log("Don't Cancel");
        Ready = false;
    }
    public void OnChangeScene()
    {
        Ready = true;
        SceneController.Instance.LoadScene("Game");
        GameEvents.Instance.OnGameScene?.Invoke(EGameScene.Game);
    }
}
