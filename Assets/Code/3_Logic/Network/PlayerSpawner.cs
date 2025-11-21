using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;

    private GameSessionManager _sessionManager;

    public void Start()
    {
        _sessionManager = FindObjectOfType<GameSessionManager>();
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkObject networkPlayerObject = Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
            Runner.SetPlayerObject(player, networkPlayerObject);
        }

        Debug.Log($"setMaster {Runner.IsSharedModeMasterClient}");
        if (Runner.IsSharedModeMasterClient)
        {
            Debug.Log($"I am the SharedModeMasterClient ({Runner.LocalPlayer}). I will now check and assign the Master role.");
            AssignMasterClientRole();
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        Runner.SetPlayerObject(player, null);

        
        if (SceneManager.GetActiveScene().name != "CreateAndJoin")
        {
            _sessionManager = FindObjectOfType<GameSessionManager>();
            _sessionManager?.HandleMasterClientLeft(Runner);
            return; 
        }

        if (Runner.IsSharedModeMasterClient)
        {
            Debug.Log($"A player left. As SharedModeMasterClient ({Runner.LocalPlayer}), I will re-assign the Master role.");
            AssignMasterClientRole();
        }
    }

    private void AssignMasterClientRole()
    {
        // We still need a deterministic way to CHOOSE the master, even if the registry order is different.
        // The PlayerId is the only reliable, consistent data point across all clients.
        PlayerNetwork designatedMaster = null;
        int lowestPlayerId = int.MaxValue;

        foreach (var pNet in PlayerRegistry.Instance.GetAllPlayers())
        {
            if (pNet.PlayerRef.PlayerId < lowestPlayerId)
            {
                lowestPlayerId = pNet.PlayerRef.PlayerId;
                designatedMaster = pNet;
            }
        }

        if (designatedMaster == null)
        {
            Debug.Log("No players in registry to assign as master.");
            return;
        }

        Debug.Log($"Designated Master is Player {designatedMaster.PlayerRef.PlayerId}");

        // Now, command every player to update their status based on our decision.
        foreach (var pNet in PlayerRegistry.Instance.GetAllPlayers())
        {
            bool shouldBeMaster = (pNet == designatedMaster);
            // We don't need to check the current status, just send the command.
            if (pNet.IsMaster != shouldBeMaster)
            {
                pNet.RPC_SetMasterClient(shouldBeMaster);
            }
        }
    }

    public void Spawned()
    {
    }
}