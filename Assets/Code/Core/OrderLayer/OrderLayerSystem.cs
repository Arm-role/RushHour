using System.Collections.Generic;
using UnityEngine;

public class OrderLayerSystem 
{
    private Dictionary<Component, int> layerEntries = new();
    public void Add(Component component)
    {
        if (component == null) return;

        if (!layerEntries.ContainsKey(component))
        {
            int sortingOrder = GetSortingOrder(component);
            layerEntries[component] = sortingOrder;
        }
    }
    public void SetOrderFront(int orderOffset = 10)
    {
        var keys = new List<Component>(layerEntries.Keys);
        foreach (var component in keys)
        {
            if (component != null)
            {
                int order = GetSortingOrder(component);
                SetSortingOrder(component, order + orderOffset);
            }
        }
    }
    public void ResetOrder()
    {
        var keys = new List<Component>(layerEntries.Keys);   
        foreach (var component in keys)
        {
            if (component == null)
            {
                layerEntries.Remove(component);
                continue;
            }
            else
            {
                SetSortingOrder(component, layerEntries[component]);
            }
        }
    }
    private int GetSortingOrder(Component c)
    {
        return c switch
        {
            SpriteRenderer s => s.sortingOrder,
            Canvas canvas => canvas.sortingOrder,
            _ => -1
        };
    }
    private void SetSortingOrder(Component c, int order)
    {
        switch (c)
        {
            case SpriteRenderer sprite:
                sprite.sortingOrder = order;
                break;
            case Canvas canvas:
                canvas.sortingOrder = order;
                break;
        }
    }
}
