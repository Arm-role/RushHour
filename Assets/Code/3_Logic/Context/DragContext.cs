using System.Collections;
using UnityEngine;

public readonly struct DragContext
{
    public readonly InteractableItem DraggedItem;
    public readonly Vector2 StartTouchPosition;
    public readonly Vector2 CurrentTouchPosition;
    public readonly float HoldMoveTolerance;
    public readonly float HoldThreshold;
    public readonly float DeltaTime;

    public readonly float CurrentHoldTimer;
    public readonly bool HasAlreadyMovedTooMuch;

    public readonly bool IsTouch;
    public readonly bool IsDoubleTouch;
    public readonly bool IsInputHold;
    public readonly bool IsInputReleased;

    public readonly Collider2D HitCollider;
    public readonly Collider2D[] HitColliders;
    public DragContext(Collider2D hitCollider, Collider2D[] hitColliders, InteractableItem draggedItem,
        Vector2 startTouchPosition, Vector2 currentTouchPosition, float holdMoveTolerance, float holdThreshold,
        float deltaTime, float currentHoldTimer, bool hasAlreadyMovedTooMuch, bool isTouch,bool isDoubleTouch, bool isInputHold, bool isInputReleased)
    {
        HitColliders = hitColliders;
        HitCollider = hitCollider;
        DraggedItem = draggedItem;

        StartTouchPosition = startTouchPosition;
        CurrentTouchPosition = currentTouchPosition;
        HoldMoveTolerance = holdMoveTolerance;
        HoldThreshold = holdThreshold;
        DeltaTime = deltaTime;

        CurrentHoldTimer = currentHoldTimer;
        HasAlreadyMovedTooMuch = hasAlreadyMovedTooMuch;

        IsTouch = isTouch;
        IsDoubleTouch = isDoubleTouch;
        IsInputHold = isInputHold;
        IsInputReleased = isInputReleased;
    }
}