using Fusion;
using ItemEvents;
using NetworkEvents;
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

    private GameState _gameState;

    public override void Spawned()
    {
        Debug.Log("Create GameObject");
        PlayerRegistry.Instance.RegisterPlayer(this);

        UpdatePlayerName();

        if (IsLocalPlayer)
        {
            EventManager.Subscribe<SentMenu>(SentMenu);
            EventManager.Subscribe<OrderExpiredEvent>(OnOrderExpired);
        }

        DontDestroyOnLoad(this);
        EventManager.Invoke(new PlayerViewSpawned(this));
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        PlayerRegistry.Instance.UnregisterPlayer(this);
        EventManager.Invoke(new PlayerViewDespawned(this));

        if (IsLocalPlayer)
        {
            EventManager.Unsubscribe<SentMenu>(SentMenu);
            EventManager.Unsubscribe<OrderExpiredEvent>(OnOrderExpired);
        }
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


    private void SentMenu(SentMenu menu)
    {
        if (_gameState == null) _gameState = FindAnyObjectByType<GameState>();

        byte[] menuID = ByteConverter.IntToBytes(menu.MenuId);
        byte[] score = ByteConverter.FloatToBytes(menu.ScoreValue);

        _gameState.RPC_SentMenu(PlayerRef.PlayerId, menuID, score);
    }
    private void OnOrderExpired(OrderExpiredEvent evt)
    {
        if (_gameState == null) _gameState = FindAnyObjectByType<GameState>();

        _gameState.RPC_PlayerOrderExpired(PlayerRef.PlayerId);
    }



    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetMasterClient(bool newMasterRef)
    {
        IsMaster = newMasterRef;
    }

    [Rpc(RpcSources.All, RpcTargets.InputAuthority)]
    public void RPC_GetOrder(byte[] orderIDb, byte[] menuIDb)
    {
        int orderID = ByteConverter.BytesToInt(orderIDb);
        int menuID = ByteConverter.BytesToInt(menuIDb);

        EventManager.Invoke(new OrderIdEjectedAndSetUp(orderID, menuID));
    }

    [Rpc(RpcSources.All, RpcTargets.InputAuthority)]
    public void RPC_GetItems(byte[] itemsData)
    {
        int[] items = ByteConverter.BytesToIntArray(itemsData);

        EventManager.Invoke(new ItemIdsEjectedLaunch(items));
    }

    #region Transfer Item

    [Rpc(RpcSources.All, RpcTargets.InputAuthority)]
    public void RPC_ReceiveItem(byte[] itemData)
    {
        int itemId = ByteConverter.BytesToInt(itemData);

        EventManager.Invoke(new ItemIdEjectedLaunch(itemId));
    }

    #endregion
}
