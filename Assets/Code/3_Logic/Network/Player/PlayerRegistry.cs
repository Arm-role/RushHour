using Fusion;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class PlayerRegistry : SingletonBase<PlayerRegistry>
{
    private readonly Dictionary<PlayerRef, PlayerNetwork> _players = new Dictionary<PlayerRef, PlayerNetwork>();
    protected PlayerRegistry() { }

    public void RegisterPlayer(PlayerNetwork player)
    {
        if (_players.TryAdd(player.PlayerRef, player))
        {
            Debug.Log($"Player registered: {player.PlayerRef.PlayerId}");
        }
    }
    public void UnregisterPlayer(PlayerNetwork player)
    {
        if (_players.Remove(player.PlayerRef))
        {
            Debug.Log($"Player unregistered: {player.PlayerRef.PlayerId}");
        }
    }

    public PlayerNetwork GetPlayer(PlayerRef playerRef)
    {
        _players.TryGetValue(playerRef, out var player);
        return player;
    }

    public PlayerNetwork GetPlayer(int playerId)
    {
        foreach (var player in _players)
        {
            if (player.Key.PlayerId == playerId)
            {
                return player.Value;
            }
        }
        return null;
    }

    public IEnumerable<PlayerNetwork> GetAllPlayers()
    {
        return _players.Values;
    }

    public PlayerNetwork GetLocalPlayer()
    {
        foreach (var player in _players.Values)
        {
            if (player.IsLocalPlayer) return player;
        }
        return null;
    }

    public PlayerNetwork GetInferredMasterClient()
    {
        if (_players.Count == 0) return null;

        return _players.Values.OrderBy(p => p.PlayerRef.PlayerId).FirstOrDefault();
    }

    public int GetPlayerIndex(PlayerNetwork targetPlayer)
    {
        var orderedPlayers = _players.Values
            .OrderBy(p => p.PlayerRef.PlayerId)
            .ToList();

        int index = orderedPlayers.IndexOf(targetPlayer);
        return index; 
    }
}