using System;
using UnityEngine;
using System.Collections.Generic;

public class SpawnEventBroker : IDisposable
{
    private readonly SpawnerHandle _spawnerHandle; 
    private readonly List<IOnSpawnListener> _spawnListeners;
    private readonly List<IOnDespawnListener> _despawnListeners;

    public SpawnEventBroker(SpawnerHandle spawnerHandle,
        IEnumerable<IOnSpawnListener> spawnListeners,
        IEnumerable<IOnDespawnListener> despawnListeners)
    {
        _spawnerHandle = spawnerHandle;
        _spawnListeners = new List<IOnSpawnListener>(spawnListeners ?? Array.Empty<IOnSpawnListener>());
        _despawnListeners = new List<IOnDespawnListener>(despawnListeners ?? Array.Empty<IOnDespawnListener>());

        _spawnerHandle.OnSpawnCompleted += HandleSpawn;
        _spawnerHandle.OnDespawnCompleted += HandleDespawn;
    }

    private void HandleSpawn(GameObject obj)
    {
        for (int i = 0; i < _spawnListeners.Count; i++)
            _spawnListeners[i].OnSpawned(obj);
    }

    private void HandleDespawn(GameObject obj)
    {
        for (int i = 0; i < _despawnListeners.Count; i++)
            _despawnListeners[i].OnDespawned(obj);
    }

    public void Dispose()
    {
        _spawnerHandle.OnSpawnCompleted -= HandleSpawn;
        _spawnerHandle.OnDespawnCompleted -= HandleDespawn;
    }
}