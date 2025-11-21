using Fusion;
using GameEvents;
using System;
using UnityEngine;

#region Networked Structs
public struct PlayerCurrentQuest : INetworkStruct
{
    public PlayerRef PlayerRef;
    public int AssignedMenuId;
}

public enum GamePhase
{
    Lobby,
    LevelSetup,
    LevelCutscene,
    Gameplay,
    LevelComplete,
    GameOver
}

#endregion

public class GameState : NetworkBehaviour
{
    public static GameState instance;

    [Networked, OnChangedRender(nameof(OnPhaseChanged))]
    public EGameFlow CurrentPhase { get; set; }

    [Networked, Capacity(6), OnChangedRender(nameof(OnSeatingOrderChanged))]
    public NetworkLinkedList<PlayerRef> SeatingOrder { get; }

    [Networked] public int RequiredCount { get; set; }
    [Networked] public int CompletedCount { get; set; }

    [Networked] public int GlobalLevelIndex { get; set; }
    [Networked] public int CurrentRunLevelIndex { get; set; }
    [Networked] public bool IsRandomLevel { get; set; }
    [Networked] public float TotalScore { get; set; }
    [Networked] public float GameTimer { get; set; }
    [Networked] public float MaxGameTime { get; set; }

    [Networked]
    public NetworkDictionary<PlayerRef, PlayerCurrentQuest> PlayerAssignments { get; }

    public event Action OnSeatingUpdated;

    public event Action<int, int, float> OnSentItem;
    public event Action<int> OnPlayerOrderExpired;

    public static event Action OnNetworkStateChanged;





    public override void Spawned()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this);
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
    }





    private void OnPhaseChanged()
    {
        Debug.Log("---------------------------" + CurrentPhase);
        GameFlowState.Set(CurrentPhase);
    }
    private void OnSeatingOrderChanged()
    {
        OnSeatingUpdated?.Invoke();
    }





    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && CurrentPhase == EGameFlow.GamePlay)
        {
            GameTimer -= Runner.DeltaTime;

            if (GameTimer <= 0)
            {
                GameTimer = 0;
                CurrentPhase = EGameFlow.GameEnd;
            }
        }

        if (Runner.Tick % 5 == 0)
        {
            RPC_FireStateUpdate();
        }
    }





    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_FireStateUpdate() => OnNetworkStateChanged?.Invoke();


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SentMenu(int playerId, byte[] MenuIdb, byte[] ScoreValueb)
    {
        int MenuID = ByteConverter.BytesToInt(MenuIdb);
        float ScoreValue = ByteConverter.BytesToFloat(ScoreValueb);

        OnSentItem?.Invoke(playerId, MenuID, ScoreValue);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_PlayerOrderExpired(int playerId)
    {
        OnPlayerOrderExpired?.Invoke(playerId);
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestItemTransfer(int itemId, PlayerRef requester, PlayerRef receiver, RpcInfo info = default)
    {
        if (requester != info.Source)
        {
            Debug.LogError($"[Security Alert] Player {info.Source} impersonated {requester}. Request denied.");
            return;
        }

        PlayerNetwork receiverNetwork = PlayerRegistry.Instance.GetPlayer(receiver);
        if (receiverNetwork == null)
        {
            Debug.LogWarning($"[Validation Failed] Receiver {receiver} not found. Request denied.");
            return;
        }

        Debug.Log($"[Request Approved] Spawning item {itemId} and delivering to Player {receiver}.");

        byte[] itemData = BitConverter.GetBytes(itemId);
        receiverNetwork.RPC_ReceiveItem(itemData);
    }







    [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
    public void RPC_RequestOrderSpawn(byte[] orderIDb, byte[] menuIDb, int targetId, RpcInfo info = default)
    {
        PlayerNetwork receiverNetwork = PlayerRegistry.Instance.GetPlayer(targetId);
        if (receiverNetwork == null)
        {
            Debug.LogWarning($"[Validation Failed] Receiver {targetId} not found. Request denied.");
            return;
        }

        receiverNetwork.RPC_GetOrder(orderIDb, menuIDb);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
    public void RPC_RequestItemSpawn(byte[] itemIDs, PlayerRef target, RpcInfo info = default)
    {
        PlayerNetwork receiverNetwork = PlayerRegistry.Instance.GetPlayer(target);
        if (receiverNetwork == null)
        {
            Debug.LogWarning($"[Validation Failed] Receiver {target} not found. Request denied.");
            return;
        }

        receiverNetwork.RPC_GetItems(itemIDs);
    }
}
