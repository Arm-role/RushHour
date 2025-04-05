using Fusion;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private Dictionary<PlayerRef, PlayerNetwork> _players;
    private Dictionary<PlayerRef, PlayerNetwork> players => _players ??= DIPlayerContain.Instance.GetPlayers() ?? new Dictionary<PlayerRef, PlayerNetwork>();

    private void Start()
    {
        textMesh.text = MaxTime.ToString();

        factory = new LobbyFactory(playerPrefab, parent);
        readySystem = new ReadySystem();
        timerSystem = new TimerSystem();

        DIPlayerContain.Instance.RegisterAddPlayerNetwork(GetPlayer);
        DILobby.Instance.RegisterReady(OnReadyChanged);

        PlayerEvents.Instance.OnReady.Invoke(false);

        foreach (var player in players.Values)
        {
            GetPlayer(player);
        }
    }
    public void GetPlayer(PlayerNetwork playerNetwork)
    {
        var playerLobby = factory.CreatePlayerLobby(playerNetwork);

        DILobby.Instance.AddPlayerLobby(playerLobby);
        DILobby.Instance.OnReadyChanged(playerNetwork.playerRef, playerNetwork.IsReady);

        if (playerNetwork.isLocalPlayer)
        {
            readyButton.onClick.AddListener(() => playerNetwork.OnSetReady(true));
            cancelButton.onClick.AddListener(() => playerNetwork.OnSetReady(false));
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
    }
}
