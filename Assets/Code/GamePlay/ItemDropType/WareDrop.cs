using System.Collections.Generic;
using System;
using UnityEngine;

public class WareDrop : IDropType
{
    public readonly Dictionary<string, Func<Collider2D, IDrop>> _tagBasedStrategies = new()
    {
        { "TrashBox", (coll) => new WareOverTrash() }
    };
    public readonly Dictionary<EItemType, Func<Collider2D, IDrop>> _itemTypeBasedStrategies = new()
    {
        { EItemType.Order, (coll) => new WareOverOrder() }
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
