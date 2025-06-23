using System;
using System.Collections.Generic;
using UnityEngine;

public class FoodDrop : IDropType
{
    public readonly Dictionary<string, Func<Collider2D, IDrop>> _tagBasedStrategies = new()
    {
        { "ArrowLeft",  (col) => new FoodOverArrowL() },
        { "ArrowRight", (col) => new FoodOverArrowR() }
    };
    public readonly Dictionary<EItemType, Func<Collider2D, IDrop>> _itemTypeBasedStrategies = new()
    {
        { EItemType.Tool, (col) => new FoodOverTool() },
        { EItemType.Ware, (col) => new FoodOverWare() }
    };

    public IDrop Resolve(Collider2D collider)
    {
        if (collider.TryGetComponent<InteractableItem>(out var target))
        {
            if (_itemTypeBasedStrategies.TryGetValue(target.itemType, out var strategy))
            {
                return strategy(collider);
            }
        }
        else
        {
            if (_tagBasedStrategies.TryGetValue(collider.tag, out var strategy))
            {
                return strategy(collider);
            }
        }
        return null;
    }
}
