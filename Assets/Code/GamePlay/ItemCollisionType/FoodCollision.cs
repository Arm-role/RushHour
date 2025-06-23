using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCollision : ICollisionType
{
    public readonly Dictionary<string, Func<Collider2D, ICollision>> _tagBasedEnterStrategies = new()
    {
    };
    public readonly Dictionary<EItemType, Func<Collider2D, ICollision>> _itemTypeBasedEnterStrategies = new()
    {
        { EItemType.Tool, (col) => new FoodCollTool() }
    };

    public ICollision EnterResolve(Collider2D collider)
    {
        if (collider.TryGetComponent<InteractableItem>(out var target))
        {
            if (_itemTypeBasedEnterStrategies.TryGetValue(target.itemType, out var strategy))
            {
                return strategy(collider);
            }
        }
        else
        {
            if (_tagBasedEnterStrategies.TryGetValue(collider.tag, out var strategy))
            {
                return strategy(collider);
            }
        }
        return null;
    }

    public readonly FoodExit foodExit = new FoodExit();
    public ICollision ExitResolve()
    {
        return foodExit;
    }
}
