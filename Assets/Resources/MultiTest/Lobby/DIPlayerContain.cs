using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DIPlayerContain
{
    private static DIPlayerContain _instance;
    public static DIPlayerContain Instance => _instance ??= new DIPlayerContain();

    private NetworkRunner runner;
    private Dictionary<PlayerRef, PlayerNetwork> playerDataMap = new Dictionary<PlayerRef, PlayerNetwork>();
    private List<Action<PlayerNetwork>> AddPlayerListener = new List<Action<PlayerNetwork>>();
    private List<Action<PlayerRef>> RemovePlayerListener = new List<Action<PlayerRef>>();

    public PlayerNetwork LocalPlayer { get; private set; }
    public void SetNetworkRunner(NetworkRunner runnerImport)
    {
        runner = runnerImport;
    }
    public NetworkRunner GetNetworkRunner()
    {
        return runner;
    }

    public void RegisterPlayer(PlayerNetwork playerNetwork)
    {
        if (playerNetwork != null && !playerDataMap.ContainsKey(playerNetwork.playerRef))
        {
            playerDataMap.Add(playerNetwork.playerRef, playerNetwork);
            Debug.Log($"📌 Registered Player: {playerNetwork.playerName} (ID: {playerNetwork.playerRef.PlayerId})");
        }

        foreach (var action in AddPlayerListener)
        {
            action(playerNetwork);
        }
        if (playerNetwork.isLocalPlayer)
        {
            LocalPlayer = playerNetwork;
        }
    }

    public void UnregisterPlayer(PlayerRef playerRef)
    {
        if (playerDataMap.ContainsKey(playerRef))
        {
            playerDataMap.Remove(playerRef);
            Debug.Log($"❌ Unregistered Player: {playerRef.PlayerId}");
        }

        foreach (var action in RemovePlayerListener)
        {
            action(playerRef);
        }
    }
    public PlayerNetwork GetPlayer(PlayerRef playerRef)
    {
        return playerDataMap.TryGetValue(playerRef, out var player) ? player : null;
    }
    public PlayerNetwork LocalPlayerNetwork()
    {
        return playerDataMap[LocalPlayer.playerRef];
    }

    public Dictionary<PlayerRef, PlayerNetwork> GetPlayers()
    {
        return playerDataMap;
    }

    public void RegisterAddPlayerNetwork(Action<PlayerNetwork> action)
    {
        AddPlayerListener.Add(action);
    }
    public void RegisterRemovePlayerNetwork(Action<PlayerRef> action)
    {
        RemovePlayerListener.Add(action);
    }
}
