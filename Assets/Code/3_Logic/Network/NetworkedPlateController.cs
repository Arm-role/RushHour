using Fusion;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(PlateLayoutManager))]
public class NetworkedPlateController : NetworkBehaviour
{
    [Header("Configuration")]
    [SerializeField] private NetworkPrefabRef _platePrefab;
    [SerializeField] private int _maxPlates = 6; // Max players/plates

    private PlateLayoutManager _layoutManager;

    [Networked, Capacity(10)]
    private NetworkLinkedList<NetworkObject> _allSpawnedPlates { get; }

    [Networked, OnChangedRender(nameof(UpdateVisuals)), Capacity(10)]
    private NetworkLinkedList<int> _activePlateKeys { get; }
    public override void Spawned()
    {
        _layoutManager = GetComponent<PlateLayoutManager>();

        if (Runner.IsSharedModeMasterClient)
        {
            RPC_PreSpawnAllPlates();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
    private void RPC_PreSpawnAllPlates()
    {
        if (_allSpawnedPlates.Count > 0) return; // Already spawned

        for (int i = 0; i < _maxPlates; i++)
        {
            NetworkObject newPlateNO = Runner.Spawn(_platePrefab, Vector3.zero, Quaternion.identity);
            if (newPlateNO != null)
            {
                PlateNetworkLobby plate = newPlateNO.GetComponent<PlateNetworkLobby>();
                plate.IsActive = false;
                _allSpawnedPlates.Add(newPlateNO);
            }
        }
    }

    public void SelectedKey(int key)
    {
        if (Runner.IsSharedModeMasterClient)
        {
            RPC_SetPlateActiveState(key, true);
        }
        else
        {
            Debug.LogWarning("Only the Master Client can activate plates.");
        }
    }
    public void RejectedKey(int key)
    {
        if (Runner.IsSharedModeMasterClient)
        {
            RPC_SetPlateActiveState(key, false);
        }
        else
        {
            Debug.LogWarning("Only the Master Client can activate plates.");
        }
    }

    public void RollKey(List<int> keysToSelect)
    {
        if (!Runner.IsSharedModeMasterClient) return;

        _activePlateKeys.Clear();
        foreach (var key in keysToSelect)
        {
            if (!_activePlateKeys.Contains(key))
            {
                _activePlateKeys.Add(key);
                Debug.Log($" Selected : {key} ");
            }
        }
    }

    public void ResetKey(List<int> keys)
    {
        if (!Runner.IsSharedModeMasterClient) return;
        _activePlateKeys.Clear();
    }

    private void UpdateVisuals()
    {
        if (_layoutManager == null) _layoutManager = GetComponent<PlateLayoutManager>();

        var activeKeysSet = new HashSet<int>(_activePlateKeys);
        List<NetworkObject> platesToArrange = new List<NetworkObject>();

        for (int i = 0; i < _allSpawnedPlates.Count; i++)
        {
            var plateNO = _allSpawnedPlates[i];
            if (plateNO == null) continue;

            var plate = plateNO.GetComponent<PlateNetworkLobby>();

            if (activeKeysSet.Contains(i))
            {
                plate.gameObject.SetActive(true);
                plate.IsActive = true;
                plate.foodSpriteId = i;
                platesToArrange.Add(plateNO);
            }
            else
            {
                plate.gameObject.SetActive(false);
                plate.IsActive = false;
            }
        }

        List<NetworkObject> orderedPlatesToArrange = new List<NetworkObject>();
        foreach (int key in _activePlateKeys)
        {
            orderedPlatesToArrange.Add(_allSpawnedPlates[key]);
        }

        _layoutManager.ArrangePlates(orderedPlatesToArrange);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetPlateActiveState(int key, bool isActive)
    {
        if (key < 0 || key >= _allSpawnedPlates.Count) return;

        NetworkObject plateNO = _allSpawnedPlates[key];
        if (plateNO == null) return;

        var plate = plateNO.GetComponent<PlateNetworkLobby>();
        plate.IsActive = isActive;

        if (isActive)
            plate.foodSpriteId = key;

        if (isActive)
        {
            if (!_activePlateKeys.Contains(key))
            {
                _activePlateKeys.Add(key);
            }
        }
        else
        {
            _activePlateKeys.Remove(key);
        }

        RPC_ArrangeVisuals();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ArrangeVisuals()
    {
        List<NetworkObject> orderedActivePlates = new List<NetworkObject>();

        foreach (int key in _activePlateKeys)
        {
            if (key >= 0 && key < _allSpawnedPlates.Count)
            {
                NetworkObject plateNO = _allSpawnedPlates[key];
                if (plateNO != null)
                {
                    orderedActivePlates.Add(plateNO);
                }
            }
        }

        Debug.Log($"Arranging {orderedActivePlates.Count} active plates in activation order.");

        _layoutManager.ArrangePlates(orderedActivePlates);
    }


}
