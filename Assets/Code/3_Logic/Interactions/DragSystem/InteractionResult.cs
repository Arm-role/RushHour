using System.Collections;
using UnityEngine;

public class InteractionResult
{
    public readonly DragStateUpdate StateUpdate;
    public readonly bool IsHold;
    public readonly bool ShouldMoveItem;
    public readonly bool CheckCollisions;
    public readonly bool ResetCollisions;
    public readonly bool ShouldClearItem;
    public readonly Collider2D ColliderToSet;
    public readonly Collider2D TargetCollider;
    public readonly ESortOrder SortOrderType;
    public InteractionResult(
        DragStateUpdate stateUpdate = null,
        bool isHold = false,
        bool shouldMoveItem = false,
        bool checkCollisions = false,
        bool resetCollisions = false,
        bool shouldClearItem = false,
        Collider2D colliderToSet = null,
        Collider2D targetCollider = null,
        ESortOrder sortOrderType = default)
    {
        StateUpdate = stateUpdate;
        IsHold = isHold;
        ShouldMoveItem = shouldMoveItem;
        CheckCollisions = checkCollisions;
        ResetCollisions = resetCollisions;
        ShouldClearItem = shouldClearItem;
        ColliderToSet = colliderToSet;
        TargetCollider = targetCollider;
        SortOrderType = sortOrderType;
    }
    public static InteractionResult SetItem(Collider2D collider)
    {
        return new InteractionResult(colliderToSet: collider);
    }
    public static InteractionResult CheckCollision(bool collision)
    {
        return new InteractionResult(checkCollisions: collision);
    }
    public static InteractionResult ResetCollision(bool collision)
    {
        return new InteractionResult(resetCollisions: collision);
    }
}