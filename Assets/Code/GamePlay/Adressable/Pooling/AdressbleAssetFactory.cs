using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AdressbleAssetFactory<T> : IAsyncFactory<T> where T : class
{
    private readonly AssetReference _assetRef;

    public AdressbleAssetFactory(AssetReference assetRef)
    {
        _assetRef = assetRef;
    }
    public async Task<(T asset, AsyncOperationHandle handle)> CreateAsync()
    {
        var handle = Addressables.LoadAssetAsync<T>(_assetRef);
        T loadedAsset = await handle.Task;

        return (loadedAsset, handle);
    }
}