using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class VisualPresenter
{
    private string _spriteObName;
    private Transform _keepper;
    private Transform _helper;
    private GameObjectSpawner _spawner;
    private ItemWorkService _itemWorkService;
    private readonly Dictionary<Item, Stack<SpriteItem>> _spriteObs = new();
    private readonly Dictionary<Item, Stack<SpriteItem>> _spriteObsHelp = new();

    public VisualPresenter(string spriteObName, Transform keepper, GameObjectSpawner spawner, Transform helper = null, ItemWorkService itemWorkService = null)
    {
        _spriteObName = spriteObName;
        _keepper = keepper;
        _helper = helper;
        _spawner = spawner;
        _itemWorkService = itemWorkService;
    }


    public async Task<SpriteItem> AddItem(Item item, int sortOrder, bool isRandomRotation = false)
    {
        GameObject spriteObject = await _spawner.SpawnOB(_spriteObName, _keepper.position);
        spriteObject.transform.SetParent(_keepper);
        spriteObject.transform.localPosition = Vector3.zero;

        if (isRandomRotation) spriteObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        else spriteObject.transform.rotation = _keepper.transform.rotation;

        if (spriteObject.TryGetComponent<SpriteItem>(out var spriteItem))
        {
            spriteItem.Renderor.sprite = item.sprite;
            spriteItem.Renderor.sortingOrder += sortOrder;
            spriteItem.OnRequestDestruction = _spawner.DespawnOB;

            if (!_spriteObs.ContainsKey(item))
            {
                _spriteObs[item] = new Stack<SpriteItem>();
            }

            _spriteObs[item].Push(spriteItem);
            return spriteItem;
        }

        return null;
    }
    public void RemoveItem(Item item, System.Action<SpriteItem> onEachRemove = null)
    {
        if (_spriteObs.TryGetValue(item, out var stack))
        {
            var spriteItem = stack.Pop();
            onEachRemove?.Invoke(spriteItem);
            spriteItem.RequestDestruction();

            if (stack.Count == 0)
            {
                _spriteObs.Remove(item);
            }
        }
    }
    public void RemoveItemAll(System.Action<SpriteItem> onEachRemove = null)
    {
        foreach (var kvp in _spriteObs)
        {
            var stack = kvp.Value;
            while (stack.Count > 0)
            {
                var spriteItem = stack.Pop();
                onEachRemove?.Invoke(spriteItem);
                spriteItem.RequestDestruction();
            }
        }
        _spriteObs.Clear();
    }

    public async Task<SpriteItem> AddHelp(Item item, int sortOrder)
    {
        if (!_itemWorkService.TryGetToolName(item.Name, out Task<Item> taskItem)) return null;

        Item toolItem = await taskItem;

        GameObject spriteObject = await _spawner.SpawnOB(_spriteObName, _helper.position);
        spriteObject.transform.SetParent(_helper);
        spriteObject.transform.localPosition = Vector3.zero;

        spriteObject.transform.rotation = _helper.transform.rotation;

        if (spriteObject.TryGetComponent<SpriteItem>(out var spriteItem))
        {
            spriteItem.Renderor.sprite = toolItem.sprite;
            spriteItem.Renderor.sortingOrder += sortOrder;
            spriteItem.OnRequestDestruction = _spawner.DespawnOB;

            if (!_spriteObsHelp.ContainsKey(toolItem))
            {
                _spriteObsHelp[toolItem] = new Stack<SpriteItem>();
            }

            _spriteObsHelp[toolItem].Push(spriteItem);
            return spriteItem;
        }

        return null;
    }
    public async void RemoveHelp(Item item, System.Action<SpriteItem> onEachRemove = null)
    {
        if (!_itemWorkService.TryGetToolName(item.Name, out Task<Item> taskItem)) return;

        Item toolItem = await taskItem;

        if (_spriteObsHelp.TryGetValue(toolItem, out var stack))
        {
            var spriteItem = stack.Pop();
            onEachRemove?.Invoke(spriteItem);
            spriteItem.RequestDestruction();

            if (stack.Count == 0)
            {
                _spriteObsHelp.Remove(toolItem);
            }
        }
    }
    public void RemoveHelpAll(System.Action<SpriteItem> onEachRemove = null)
    {
        foreach (var kvp in _spriteObsHelp)
        {
            var stack = kvp.Value;
            while (stack.Count > 0)
            {
                var spriteItem = stack.Pop();
                onEachRemove?.Invoke(spriteItem);
                spriteItem.RequestDestruction();
            }
        }
        _spriteObsHelp.Clear();
    }
}