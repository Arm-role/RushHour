﻿using Fusion;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
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

    private IPlayerNetworkState currentState;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        ChangeState(new PlayerLobbyState());
    }
    void isReadyChanged()
    {
        DILobby.Instance.OnReadyChanged(playerRef, IsReady);
    }
    void isScoreChange()
    {
        Debug.Log(playerName + " " + MyScore);
        EvenManager.OnSentTotalScore(currentScore);
    }

    void Update()
    {
        if (HasStateAuthority == false)
        {
            return;
        }
        currentState?.Execute(this);
    }
    public void ChangeState(IPlayerNetworkState playerNet)
    {
        currentState?.Exit(this);
        currentState = playerNet;
        currentState.Enter(this);
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
        // ✅ ลบข้อมูลผู้เล่นเมื่อออกจากเกม
        DIPlayerContain.Instance.UnregisterPlayer(playerRef);
    }
    public void Ready(bool isReady) => IsReady = isReady;


    #region Score
    public void GetScore(float score)
    {
        currentScore = score;
        MyScore += score;
    }
    public float GetCurrentScore()
    {
        return MyScore;
    }
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
            EvenManager.OnSentItem(item);
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
                EvenManager.OnSentItem(item);
            }
            Debug.Log(playerName + " " + item.Name);
        }
    }
    #endregion
}
