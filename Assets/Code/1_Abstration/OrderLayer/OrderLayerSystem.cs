using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderLayerSystem
{
    private readonly Dictionary<Component, int> _layerEntries = new();
    public void Add(Component component)
    {
        if (component == null) return;

        if (!_layerEntries.ContainsKey(component))
        {
            int sortingOrder = GetSortingOrder(component);
            _layerEntries[component] = sortingOrder;
        }
    }
    public void Remove(Component component)
    {
        if (component == null) return;

        if (_layerEntries.ContainsKey(component))
        {
            _layerEntries.Remove(component);
        }
    }
    public void SetAllOrderToFront(int orderOffset = 100)
    {
        foreach (var component in _layerEntries.Keys)
        {
            if (component != null)
            {
                int order = GetSortingOrder(component);
                SetSortingOrder(component, order + orderOffset);
            }
        }
    }
    public void ResetAllOrder()
    {
        foreach (var component in _layerEntries.Keys)
        {
            if (component == null)
            {
                _layerEntries.Remove(component);
                continue;
            }
            else
            {
                SetSortingOrder(component, _layerEntries[component]);
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

    public int GetHighSortingOrder()
    {
        int higher = -100;

        foreach (var component in _layerEntries.Keys)
        {
            if (component == null)
            {
                _layerEntries.Remove(component);
                continue;
            }
            else
            {
                int sortingOrder = GetSortingOrder(component);

                if (sortingOrder > higher)
                {
                    higher = sortingOrder;
                }
            }
        }

        return higher;
    }
}
