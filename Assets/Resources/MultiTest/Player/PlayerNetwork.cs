using Fusion;
using UnityEngine;

public class PlayerNetwork : Network_StateMachine<PlayerNetwork>
{
    [Networked] public string PlayerName { get; private set; }
    [Networked] public PlayerRef PlayerRef { get; private set; }

    [Networked, OnChangedRender(nameof(OnReadyChanged))]
    public bool IsReady { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnScoreChange))]
    private float Score { get; set; }

    public bool isLocalPlayer => Object.HasInputAuthority;

    private ReceiveItem _receiveItem;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        _receiveItem = new ReceiveItem();

        if (!isLocalPlayer) return;
        SetState(new PlayerLobbyState());
    }
    void Update()
    {
        if (HasStateAuthority == false) return;
        Execute();
    }
    public override void Spawned()
    {
        DIPlayerContain.Instance.RegisterPlayer(this);

        if (!isLocalPlayer) return;

        PlayerName = LoginManager.PlayerName;
        PlayerRef = Object.InputAuthority;
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        DIPlayerContain.Instance.UnregisterPlayer(PlayerRef);
        PlayerEvents.Instance.OnKeepPlayerScore.GetParamiter().Add(PlayerName, Score);

        currentState?.Exit(this);
    }


    void OnReadyChanged()
    {
        DILobby.Instance.OnReadyChanged(PlayerRef, IsReady);
    }
    void OnScoreChange()
    {
        GameEvents.Instance.OnSentTotalScore.Invoke(Score);
    }


    #region Public API

    public void SetReady(bool isReady) => IsReady = isReady;
    public void SetScore(float score) => Score = score;
    public void AddScore(float score) => Score += score;
    public float GetScore() => Score;

    #endregion


    #region Transfer Item

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendItem(PlayerRef player, int ID) => _receiveItem?.RPC_SendItem(player, ID);
    public void OnReceiveItem(int ID) => _receiveItem?.OnReceiveItem(ID);

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendItems(PlayerRef player, byte[] ID) => _receiveItem?.RPC_SendItems(player, ID);
    public void OnReceiveItems(byte[] ID) => _receiveItem?.OnReceiveItems(ID);

    #endregion
}