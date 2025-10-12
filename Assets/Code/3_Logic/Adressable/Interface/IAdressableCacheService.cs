using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

public interface IAdressableCacheService
{
    Task<T> AsyncGet<T>(AssetReference assetRef) where T : class;

    void ReleaseAsset(AssetReference assetRef);
    void ReleaseAssetAll();
}