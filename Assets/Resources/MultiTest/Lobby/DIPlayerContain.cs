using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DIPlayerContain : SingletonBase<DIPlayerContain>
{
    private readonly Dictionary<PlayerRef, PlayerNetwork> _players = new();

    public event Action<PlayerNetwork> OnPlayerAdd;
    public event Action<PlayerRef> OnPlayerRemove;

    public PlayerNetwork LocalPlayer { get; private set; }

    protected DIPlayerContain() { }

    public void RegisterPlayer(PlayerNetwork player)
    {
        if (player == null && _players.ContainsKey(player.PlayerRef)) return;

        _players.Add(player.PlayerRef, player);


        if (player.isLocalPlayer)
        {
            LocalPlayer = player;
        }
        Debug.Log($"📌 Registered Player: {player.PlayerName} (ID: {player.PlayerRef.PlayerId})");

        OnPlayerAdd?.Invoke(player);
    }
    public void UnregisterPlayer(PlayerRef playerRef)
    {
        if (_players.Remove(playerRef))
        {
            OnPlayerRemove?.Invoke(playerRef);
            Debug.Log($"❌ Unregistered Player: {playerRef.PlayerId}");
        }
    }


    public PlayerNetwork GetPlayer(PlayerRef playerRef) => _players.TryGetValue(playerRef, out var player) ? player : null;
    public IReadOnlyDictionary<PlayerRef, PlayerNetwork> GetAllPlayers() => _players;
}
