using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AdressablePoolingService : IAdressablePoolService
{
    private readonly Dictionary<AssetReference, AsyncObjectPool<GameObject>> _pool = new();
    private readonly Transform _poolParent;

    public AdressablePoolingService()
    {
        _poolParent = new GameObject("[AdressablePoolingService_Root]").transform;
        Object.DontDestroyOnLoad(_poolParent.gameObject);
    }

    public async Task<GameObject> AsyncGet(AssetReference assetRef)
    {
        if (!_pool.TryGetValue(assetRef, out var pool))
        {
            var factory = new AdressbleGameObjectFactory(assetRef, _poolParent);
            pool = new AsyncObjectPool<GameObject>(factory);
            _pool.Add(assetRef, pool);
        }

        return await pool.GetAsync();
    }

    public void Return(AssetReference assetRef, GameObject instance)
    {
        if (_pool.TryGetValue(assetRef, out var pool))
        {
            instance.SetActive(false);
            pool.Return(instance);
        }
        else
        {
            Debug.LogWarning($"Trying to return object to a non-existent pool: {assetRef.AssetGUID}. Destroying instead.");
            Object.Destroy(instance);
        }
    }
}