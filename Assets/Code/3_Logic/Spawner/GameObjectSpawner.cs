using System.Threading.Tasks;
using UnityEngine;

public class GameObjectSpawner
{
    private readonly IAdressablePoolService _poolService;
    private readonly GameObjectLibrary _gameObjectLibrary;

    public GameObjectSpawner(IAdressablePoolService poolService, GameObjectLibrary itemLibrary)
    {
        _poolService = poolService;
        _gameObjectLibrary = itemLibrary;
    }

    public async Task<GameObject> SpawnOB(string itemName, Vector3 position)
    {
        var assetRef = _gameObjectLibrary.Find(itemName);

        GameObject instance = await _poolService.AsyncGet(assetRef);
        instance.name = itemName;
        instance.transform.position = position;
        instance.SetActive(true);
        return instance;
    }
    public async Task<GameObject> SpawnOB(int id, Vector3 position)
    {
        var assetRef = _gameObjectLibrary.Find(id);
        if (assetRef == null || !assetRef.RuntimeKeyIsValid()) return null;

        GameObject instance = await _poolService.AsyncGet(assetRef);
        instance.transform.position = position;
        instance.SetActive(true);
        return instance;
    }

    public async Task<GameObject> SpawnOB(string itemName)
    {
        var assetRef = _gameObjectLibrary.Find(itemName);

        GameObject instance = await _poolService.AsyncGet(assetRef);
        instance.name = itemName;
        instance.SetActive(true);
        return instance;
    }
    public async Task<GameObject> SpawnOB(int id)
    {
        var assetRef = _gameObjectLibrary.Find(id);
        if (assetRef == null || !assetRef.RuntimeKeyIsValid()) return null;

        GameObject instance = await _poolService.AsyncGet(assetRef);
        instance.SetActive(true);
        return instance;
    }

    public void DespawnOB(GameObject Ob)
    {
        var assetRef = _gameObjectLibrary.Find(Ob.name);
        if (assetRef != null)
        {
            _poolService.Return(assetRef, Ob);
        }
    }
}
