using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AdressableCacheService : IAdressableCacheService
{
    private readonly Dictionary<AssetReference, AsyncObjectCache<object>> _cachedAssets = new();

    public async Task<T> AsyncGet<T>(AssetReference assetRef) where T : class
    {
        if (_cachedAssets.TryGetValue(assetRef, out var cacheAsset))
        {
            return await cacheAsset.GetAsync() as T;
        }

        if (!assetRef.RuntimeKeyIsValid()) return null;

        var factory = new AdressbleAssetFactory<object>(assetRef);
        var cache = new AsyncObjectCache<object>(factory);
        _cachedAssets.Add(assetRef, cache);

        return await cache.GetAsync() as T;
    }

    public void ReleaseAsset(AssetReference assetRef)
    {
        if (_cachedAssets.TryGetValue(assetRef, out var cacheAsset))
        {
            cacheAsset.ReleaseAsset();
        }
    }

    public void ReleaseAssetAll()
    {
        foreach (var cacheAsset in _cachedAssets.Values)
        {
            cacheAsset?.ReleaseAsset();
        }
        _cachedAssets.Clear();
    }
}