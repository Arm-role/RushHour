using System.Threading.Tasks;
using UnityEngine;

public class ItemSpawner
{
    private readonly IAdressablePoolService _poolService;
    private readonly ItemLibrary _itemLibrary;

    public ItemSpawner(IAdressablePoolService poolService, ItemLibrary itemLibrary)
    {
        _poolService = poolService;
        _itemLibrary = itemLibrary;
    }

    public async Task<InteractableItem> SpawnItem(string itemName, Vector3 position)
    {
        var assetRef = _itemLibrary.Find2(itemName);

        GameObject instance = await _poolService.AsyncGet(assetRef);
        instance.transform.position = position;
        instance.SetActive(true);
        var interactableItem = instance.GetComponent<InteractableItem>();

        return interactableItem;
    }
    public async Task<InteractableItem> SpawnItem(int id, Vector3 position)
    {
        var assetRef = _itemLibrary.Find2(id);
        if (assetRef == null || !assetRef.RuntimeKeyIsValid()) return null;

        GameObject instance = await _poolService.AsyncGet(assetRef);
        instance.transform.position = position;
        instance.SetActive(true);
        var interactableItem = instance.GetComponent<InteractableItem>();

        return interactableItem;
    }
    
    public void DespawnItem(InteractableItem item)
    {
        Debug.Log("DespawnItem");
        var assetRef = _itemLibrary.Find2(item.Item.Name);
        if(assetRef != null)
        {
            _poolService.Return(assetRef, item.gameObject);
        }
    }
}
