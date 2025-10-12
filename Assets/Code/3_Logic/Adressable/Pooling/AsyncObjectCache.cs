using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AsyncObjectCache<T> where T : class
{
    private T _cachedAsset;
    private Task<T> _loadingTask;
    private AsyncOperationHandle _assetHandle;
    private readonly IAsyncFactory<T> _factory;
    public AsyncObjectCache(IAsyncFactory<T> factory)
    {
        _factory = factory;
    }
    public Task<T> GetAsync()
    {
        if (_cachedAsset != null)
        {
            return Task.FromResult(_cachedAsset);
        }

        if (_loadingTask != null)
        {
            return _loadingTask;
        }

        _loadingTask = LoadAndCacheAssetAsync();
        return _loadingTask;
    }

    private async Task<T> LoadAndCacheAssetAsync()
    {
        (var asset, var handle) = await _factory.CreateAsync();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _cachedAsset = asset;
            _assetHandle = handle;
            _loadingTask = null;
            return _cachedAsset;
        }

        _loadingTask = null;
        return null;
    }

    public void ReleaseAsset()
    {
        if (_assetHandle.IsValid())
        {
            Addressables.Release(_assetHandle);
        }
        _cachedAsset = null;
    }
}