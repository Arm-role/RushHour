using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AdressbleGameObjectFactory : IAsyncGameObjectFactory<GameObject>
{
    private readonly AssetReference _assetRef;
    private readonly Transform _parent;

    public AdressbleGameObjectFactory(AssetReference assetRef, Transform parent)
    {
        _assetRef = assetRef;
        _parent = parent;
    }

    public async Task<GameObject> CreateAsync()
    {
        GameObject instance = await _assetRef.InstantiateAsync(_parent).Task;
        return instance;
    }
}