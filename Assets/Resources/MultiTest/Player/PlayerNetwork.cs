using Fusion;
using UnityEngine;

public class PlayerNetwork : Network_StateMachine<PlayerNetwork>
{
    private string _name;
    [Networked]
    public string playerName
    {
        get
        { return _name; }
        set
        {
            _name = value;
            gameObject.name = _name;
        }
    }

    #region Lobby

    [Networked, OnChangedRender(nameof(isReadyChanged))]
    public bool IsReady { get; set; } = false;

    #endregion

    [Networked, OnChangedRender(nameof(isScoreChange))]
    public float MyScore { get; set; }
    [Networked]
    private float currentScore { get; set; }

    public PlayerRef playerRef { get; private set; }
    public bool isLocalPlayer => Object.HasInputAuthority;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        SetState(new PlayerLobbyState());
    }
    void isReadyChanged()
    {
        DILobby.Instance.OnReadyChanged(playerRef, IsReady);
    }
    void isScoreChange()
    {
        GameEvents.Instance.OnSentTotalScore.Invoke(currentScore);
    }

    void Update()
    {
        if (HasStateAuthority == false) return;
        Execute();
    }

    public override void Spawned()
    {
        if (isLocalPlayer)
        {
            playerName = LoginManager.PlayerName;
            playerRef = Object.InputAuthority;
        }

        DIPlayerContain.Instance.RegisterPlayer(this);
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        DIPlayerContain.Instance.UnregisterPlayer(playerRef);
    }

    public void OnSetReady(bool isReady) => IsReady = isReady;
    public void OnSetScore(float score) => MyScore = score;

    #region Score
    public void GetScore(float score)
    {
        currentScore = score;
        MyScore += score;
    }
    public float GetCurrentScore() => MyScore;

    #endregion


    #region Transfer Item
    public void TransportItem(PlayerNetwork targetnetwork, Item item)
    {
        if (HasInputAuthority)
        {
            RPC_SendItem(targetnetwork.playerRef, item.ID);
        }
        else
        {
            Debug.LogWarning("TransportStatus can only be called by the State Authority.");
        }
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendItem(PlayerRef player, int ID)
    {
        if (player == playerRef)
        {
            ReceiveItem(ID);
        }
    }
    public void ReceiveItem(int ID)
    {
        Item item = ScriptTable_Contain.instance.GetItem(ID);
        if (item != null)
        {
            ItemEvents.Instance.OnSentItem.Invoke(item);
        }
    }

    public void TransportItems(PlayerNetwork targetnetwork, byte[] item)
    {
        if (HasInputAuthority)
        {
            RPC_SendItems(targetnetwork.playerRef, item);
        }
        else
        {
            Debug.LogWarning("TransportStatus can only be called by the State Authority.");
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendItems(PlayerRef player, byte[] ID)
    {
        if (playerRef == player)
        {
            ReceiveItem(ID);
        }
    }
    public void ReceiveItem(byte[] ID)
    {
        int[] IDs = IntConverter.UnpackRLEIDs(ID);

        foreach (int id in IDs)
        {
            Item item = ScriptTable_Contain.instance.GetItem(id);
            if (item != null)
            {
                ItemEvents.Instance.OnSentItem.Invoke(item);
            }
        }
    }
    #endregion
}
