using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionService
{
    public readonly Dictionary<EItemType, IDropType> dropTypes = new Dictionary<EItemType, IDropType>
    {
        {EItemType.Food, new FoodDrop() },
        {EItemType.Ware, new WareDrop() }
    };
    public IDrop GetDropResolve(EItemType itemType, Collider2D collider)
    {
        if (dropTypes.TryGetValue(itemType, out IDropType dropType))
        {
            return dropType.Resolve(collider);
        }

        Debug.LogWarning("Not Found DropType");
        return null;
    }
    public readonly Dictionary<EItemType, ICollisionType> CollisioTypes = new Dictionary<EItemType, ICollisionType>
    {
        {EItemType.Food, new FoodCollision() },
    };

    public ICollision GetCollisionEnterResolve(EItemType itemType, Collider2D collider)
    {
        if (CollisioTypes.TryGetValue(itemType, out ICollisionType collType))
        {
            return collType.EnterResolve(collider);
        }

        Debug.LogWarning("Not Found CollisionType");
        return null;
    }
    public ICollision GetCollisionExitResolve(EItemType itemType)
    {
        if (CollisioTypes.TryGetValue(itemType, out ICollisionType collType))
        {
            return collType.ExitResolve();
        }

        //Debug.LogWarning("Not Found CollisionType");
        return null;
    }
}
