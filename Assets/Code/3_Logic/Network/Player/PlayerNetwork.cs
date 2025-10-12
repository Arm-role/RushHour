using Fusion;
using ItemEvents;
using NetworkEvents;
using NUnit.Framework.Interfaces;
using System;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    // Properties for easy access
    public PlayerRef PlayerRef => Object.InputAuthority;
    public bool IsLocalPlayer => Object.HasInputAuthority;

    [Networked, OnChangedRender(nameof(UpdatePlayerName))]
    public bool IsMaster { get; set; }

    [Networked, OnChangedRender(nameof(OnScoreChanged))]
    public float Score { get; set; }

    public override void Spawned()
    {
        Debug.Log("Create GameObject");
        PlayerRegistry.Instance.RegisterPlayer(this);

        UpdatePlayerName();
        
        if (IsLocalPlayer)
        {

        }

        DontDestroyOnLoad(this);
        EventManager.Invoke(new PlayerViewSpawned(this));
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        PlayerRegistry.Instance.UnregisterPlayer(this);
        EventManager.Invoke(new PlayerViewDespawned(this));
    }

    private void OnScoreChanged()
    {
        Debug.Log($"OnChangedRender: Score for player {Object.InputAuthority} is now {Score}");

        EventManager.Invoke(new PlayerScoreUpdate(PlayerRef, Score));
    }
    private void UpdatePlayerName()
    {
        if (IsMaster)
        {
            gameObject.name = $"Player [{PlayerRef.PlayerId}][Master]";
        }
        else
        {
            gameObject.name = $"Player [{PlayerRef.PlayerId}]";
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetMasterClient(bool newMasterRef)
    {
        IsMaster = newMasterRef;
    }

    #region Transfer Item

    [Rpc(RpcSources.All, RpcTargets.InputAuthority)]
    public void RPC_ReceiveItem(byte[] itemData)
    {
        int itemId = BitConverter.ToInt32(itemData, 0);

        EventManager.Invoke(new ItemIdEjectedLaunch(itemId));
        Debug.Log($"I, {PlayerRef}, have received item {itemId}!");
    }

    #endregion
}
