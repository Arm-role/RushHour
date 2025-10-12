using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeReference, SubclassSelector] public StationDataComponent StationData;
    [SerializeField] private InteractionStrategy[] _interactionStrategie;
    [SerializeField] private InteractionStrategy[] _nullInteractionStrategie;

    [HideInInspector] public AssetProvider<Item> CacheItem;

    public StationWorker worker { get; private set; }
    private void Awake()
    {
        worker = gameObject.AddComponent<StationWorker>();
        worker.enabled = false;
    }
    public void Initialze(AssetProvider<Item> cacheItem)
    {
        CacheItem = cacheItem;
    }
    public async Task<bool> Interact(InteractableItem sourceItem)
    {
        if(_interactionStrategie.Length == 0) return false;

        IInteractionStrategy bastStrategy = null;
        int highesPriority = 0;

        foreach (IInteractionStrategy strategy in _interactionStrategie)
        {
            int currentPrivority = strategy.GetExecutionPriority(sourceItem, this);
            if (currentPrivority > highesPriority)
            {
                highesPriority = currentPrivority;
                bastStrategy = strategy;
            }
        }

        if (bastStrategy != null)
        {
            return await bastStrategy.Execute(sourceItem, this);
        }

        return false;
    }
    public async Task<bool> Interact()
    {
        if(_nullInteractionStrategie.Length == 0) return false;

        IInteractionStrategy bastStrategy = null;
        int highesPriority = 0;

        foreach (var strategy in _nullInteractionStrategie)
        {
            int currentPrivority = strategy.GetExecutionPriority(null, this);
            if (currentPrivority > highesPriority)
            {
                highesPriority = currentPrivority;
                bastStrategy = strategy;
            }
        }

        if (bastStrategy != null)
        {
            return await bastStrategy.Execute(null, this);
        }

        return false;
    }
    public bool TryGetData<T>(out T result) where T : StationDataComponent
    {
        result = default;

        if (StationData is not T) return false;
        result = (T)StationData;
        return true;
    }
    public T GetData<T>() where T : StationDataComponent
    {
        if (StationData is not T) return default;
        return (T)StationData;
    }
}