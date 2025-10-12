using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public interface IAdressablePoolService
{
    Task<GameObject> AsyncGet(AssetReference assetRef);
    
    void Return(AssetReference assetRef, GameObject instance);
}