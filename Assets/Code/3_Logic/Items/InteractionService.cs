using System.Collections.Generic;
using UnityEngine;

public class InteractionService
{
    public readonly Dictionary<string, IHold> holdTypes = new Dictionary<string, IHold>
    {
        {"Order",  new HoldOrder()}
    };

    public IHold GetHoldResolve(string itemName)
    {
        if (holdTypes.TryGetValue(itemName, out IHold holdType))
        {
            return holdType;
        }

        //Debug.LogWarning("Not Found CollisionType");
        return null;
    }

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




    public readonly Dictionary<EItemType, ICollisionType> CollisionTypes = new Dictionary<EItemType, ICollisionType>
    {
        {EItemType.Food, new FoodCollision() },
        {EItemType.Ware, new WareCollision() },
    };

    public ICollision GetCollisionEnterResolve(EItemType itemType, Collider2D collider)
    {
        if (CollisionTypes.TryGetValue(itemType, out ICollisionType collType))
        {
            return collType.EnterResolve(collider);
        }

        //Debug.LogWarning("Not Found CollisionType");
        return null;
    }

    public ICollision GetCollisionExitResolve(EItemType itemType)
    {
        if (CollisionTypes.TryGetValue(itemType, out ICollisionType collType))
        {
            return collType.ExitResolve();
        }

        //Debug.LogWarning("Not Found CollisionType");
        return null;
    }
}
