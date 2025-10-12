using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetProvider<T> where T : ScriptableObject
{
    private IAdressableCacheService _cacheService;
    private readonly ItemLibrary _itemLibrary;

    public AssetProvider(IAdressableCacheService cacheService, ItemLibrary itemLibrary)
    {
        _cacheService = cacheService;
        _itemLibrary = itemLibrary;
    }

    public async Task<T> Get(string assetName)
    {
        var assetRef = _itemLibrary.Find1(assetName);
        return  await _cacheService.AsyncGet<T>(assetRef);
    }
    public async Task<T> Get(AssetReference assetRef)
    {
        return await _cacheService.AsyncGet<T>(assetRef);
    }
    public void ReleaseAsset(AssetReference assetRef)
    {
        _cacheService.ReleaseAsset(assetRef);
    }

    public void ReleaseAssetAll()
    {
        _cacheService.ReleaseAssetAll();
    }
}
