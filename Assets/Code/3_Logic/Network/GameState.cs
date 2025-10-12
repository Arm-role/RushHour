using Fusion;
using NetworkEvents;
using System;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

#region Networked Structs
public struct PlayerSessionData : INetworkStruct
{
    public PlayerRef PlayerRef;
    public NetworkBool IsReady;
    // public int Score;         
}

public struct PlayerQuestData : INetworkStruct
{
    public PlayerRef PlayerRef;
    public int RecipeId;
    public NetworkBool IsComplete;
}

public struct IngredientLocation : INetworkStruct
{
    public int IngredientId;
    public PlayerRef LocatedAtPlayer;
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
    [Networked, Capacity(4), OnChangedRender(nameof(OnSeatingOrderChanged))]
    public NetworkLinkedList<PlayerRef> SeatingOrder { get; }

    public event Action OnSeatingUpdated;

    private void OnSeatingOrderChanged()
    {
        OnSeatingUpdated?.Invoke();
    }
    public override void Spawned()
    {
        DontDestroyOnLoad(this);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestItemTransfer(int itemId, PlayerRef requester, PlayerRef receiver, RpcInfo info = default)
    {
        if (requester != info.Source)
        {
            Debug.LogError($"[Security Alert] Player {info.Source} impersonated {requester}. Request denied.");
            return;
        }

        // --- 2. LOGIC VALIDATION (New, simplified rules) ---

        // A. Does the receiver exist?
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
}
