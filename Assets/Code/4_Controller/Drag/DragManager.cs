using System;
using UnityEngine;
using System.Collections.Generic;
using GameEvents;
using ItemEvents;

public class DragManager : MonoBehaviour
{
    private Collider2D _currentCollider;
    private Rigidbody2D _currentRB;
    private InteractableItem _interactableItem;

    private IDrag _currentState;
    private Vector2 _startDragPosition;

    private HashSet<Collider2D> _currentColliisions = new();

    private InteractionService _interactionService = new();

    #region dragState

    [Header("Drag Settings")]
    [SerializeField] private float speed = 60f;

    [SerializeField] private float holdThreshold = 0.5f;
    [SerializeField] private float holdMoveTolerance = 0.5f;

    private float _holdTimer = 0f;
    private bool _hasMovedTooMuch = false;

    public Vector2 lastTouchPos;

    private bool _isActive = false;

    #endregion

    private void Start()
    {
        SetState(new Idle_DragState());
        EventManager.Subscribe<GameFlow>(OnGameState);
    }
    private void OnDestroy() => EventManager.Unsubscribe<GameFlow>(OnGameState);

    public void OnGameState(GameFlow evt)
    {
        _isActive = (evt.Flow == EGameFlow.GamePlay);
    }

    #region Process
    private void Update()
    {
        if (!_isActive) return;
        if (_currentState == null) return;

        var context = new DragContext(
         draggedItem: _interactableItem,
         startTouchPosition: _startDragPosition,
         currentTouchPosition: GetTouchPos(),
         holdMoveTolerance: holdMoveTolerance,
         holdThreshold: holdThreshold,
         deltaTime: Time.deltaTime,
         currentHoldTimer: _holdTimer,
         hasAlreadyMovedTooMuch: _hasMovedTooMuch,
         isTouch: InputHandle.GetInputButtonDown(),
         isDoubleTouch: InputHandle.GetDoubleTouch(),
         isInputHold: InputHandle.GetInputButton(),
         isInputReleased: InputHandle.GetInputButtonUp(),
         hitCollider: GetColliderAtTouchPoint(),
         hitColliders: GetAllCollidersAtTouchPoint()
     );

        StateExecutionResult result = _currentState.OnExecute(context);
        ProcessStateResult(result);
    }
    private void SetState(IDrag newState)
    {
        if (_currentState != null)
        {
            var exitResult = _currentState.OnExit();
            ProcessInteractionResult(exitResult, _interactableItem);
        }

        _currentState = newState;

        if (_currentState != null)
        {
            var enterResult = _currentState.OnEnter();
            ProcessInteractionResult(enterResult, _interactableItem);
        }
    }

    #endregion
    private void ProcessStateResult(StateExecutionResult result)
    {
        if (result == null) return;

        if (result.InteractionResult != null)
        {
            ProcessInteractionResult(result.InteractionResult, _interactableItem);
        }
        if (result.NextState != null)
        {
            SetState(result.NextState);
        }
    }
    private void ProcessInteractionResult(InteractionResult result, InteractableItem interactableItem)
    {
        if (result == null) return;

        if (result.StateUpdate != null)
        {
            if (result.StateUpdate.NewHoldTimer.HasValue)
                _holdTimer = result.StateUpdate.NewHoldTimer.Value;
            if (result.StateUpdate.NewHasMovedTooMuch.HasValue)
                _hasMovedTooMuch = result.StateUpdate.NewHasMovedTooMuch.Value;
        }
        if (result.ShouldMoveItem) MoveItem();
        if (result.CheckCollisions) CheckCollisions();
        if (result.ResetCollisions) ResetCollisions();
        if (result.ShouldClearItem) ClearItem();
        if (result.ColliderToSet != null) SetItem(result.ColliderToSet);
        if (result.IsHold) HoldItem();

        if (result.TargetCollider != null)
        {
            var targetcoll = result.TargetCollider;
            IDrop drop = _interactionService.GetDropResolve(interactableItem.itemType, targetcoll);

            if (drop == null) return;
            var dropResult = drop.Execute(interactableItem);

            ProcessDropResult(dropResult, interactableItem, targetcoll);
        }

        if (result.SortOrderType != ESortOrder.None)
        {
            switch (result.SortOrderType)
            {
                case ESortOrder.Front:
                    interactableItem.Drag();
                    break;
                case ESortOrder.Reset:
                    interactableItem.Release();
                    break;
            }
        }
    }

    private void ProcessHoldResult(HoldExecutionResult result, InteractableItem sourceObject)
    {
        if (result == null) return;

        if (result.SourceInteraction != null)
        {
            result.SourceInteraction.Invoke(sourceObject);
        }

        if (result.ParticleToPlay != null)
        {
            EventManager.Invoke(new PlayParticle(result.ParticleToPlay, sourceObject.transform.position));
        }

        if (result.ShouldDestroySelf)
        {
            sourceObject.RequestDestruction();
        }
    }
    private void ProcessCollisionResult(CollisionExecutionResult result, InteractableItem sourceObject, Collider2D targetCollider)
    {
        if (result == null) return;

        if (result.TargetInteraction != null)
        {
            result.TargetInteraction.Invoke(targetCollider);
        }

        if (result.SourceInteraction != null)
        {
            result.SourceInteraction.Invoke(sourceObject);
        }
    }
    private async void ProcessDropResult(DropExecutionResult result, InteractableItem sourceObject, Collider2D targetCollider)
    {
        if (result == null) return;

        if (result.SourceInteraction != null)
        {
            result.SourceInteraction.Invoke(sourceObject);
        }

        if (result.TargetInteraction != null)
        {
            result.TargetInteraction.Invoke(targetCollider);
        }

        if (result.ParticleToPlay != null)
        {
            EventManager.Invoke(new PlayParticle(result.ParticleToPlay, sourceObject.transform.position));
        }
        bool destroySelf = await result.ShouldDestroySelf;
        bool destroyTarget = await result.ShouldDestroyTarget;

        if (destroySelf)
        {
            sourceObject.RequestDestruction();
        }
        if (destroyTarget)
        {
            if (targetCollider.TryGetComponent<InteractableItem>(out var targetObject))
            {
                targetObject.RequestDestruction();
            }
        }
    }


    private Vector2 GetTouchPos()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(InputHandle.GetTouchPosition());
        return new Vector2(mouseWorldPos.x, mouseWorldPos.y);
    }
    private Collider2D GetColliderAtTouchPoint()
    {
        var colliders = GetAllCollidersAtTouchPoint();
        return colliders.Length > 0 ? colliders[^1] : null;
    }
    private Collider2D[] GetAllCollidersAtTouchPoint()
    {
        Vector2 touchPos = GetTouchPos();
        var colliders = Physics2D.OverlapPointAll(touchPos);

        if (colliders.Length <= 1)
            return colliders;

        Array.Sort(colliders, (a, b) =>
        {
            var srA = a.GetComponent<SpriteRenderer>();
            var srB = b.GetComponent<SpriteRenderer>();

            // ✅ ถ้ามี SpriteRenderer ทั้งคู่
            if (srA != null && srB != null)
            {
                int layerCompare = srA.sortingLayerID.CompareTo(srB.sortingLayerID);
                if (layerCompare != 0) return layerCompare; // layer สูงกว่า → อยู่บน

                int orderCompare = srA.sortingOrder.CompareTo(srB.sortingOrder);
                if (orderCompare != 0) return orderCompare; // order สูงกว่า → อยู่บน
            }

            // ✅ ถ้ามีแค่ตัวใดตัวหนึ่งมี SpriteRenderer → ให้ตัวนั้นอยู่บน
            if (srA != null && srB == null) return 1;
            if (srA == null && srB != null) return -1;

            // ✅ ถ้าไม่มี SpriteRenderer ทั้งคู่ → ใช้ตำแหน่ง z แทน
            return (-a.transform.position.z).CompareTo(-b.transform.position.z);
        });

        return colliders;
    }

    private void SetItem(Collider2D hitColl)
    {
        if (hitColl.TryGetComponent(out _interactableItem))
        {
            _interactableItem.TryGetComponent(out _currentRB);
            _startDragPosition = GetTouchPos();
            _currentCollider = hitColl;
        }
    }
    private void ClearItem()
    {
        _currentRB = null;
        _interactableItem = null;
    }

    private void HoldItem()
    {
        IHold enter = _interactionService.GetHoldResolve(_interactableItem.Item.Name);
        if (enter == null) return;
        var result = enter.Execute(_interactableItem);

        ProcessHoldResult(result, _interactableItem);
    }

    private void MoveItem()
    {
        Vector2 dir = Vector2.MoveTowards(_currentRB.position, GetTouchPos(), speed * Time.fixedDeltaTime);
        _currentRB.MovePosition(dir);
    }
    private void CheckCollisions()
    {
        var coll = _currentCollider;
        if (coll == null) return;

        var filter = new ContactFilter2D();
        filter.useTriggers = false;

        Collider2D[] results = new Collider2D[10];
        int count = coll.OverlapCollider(filter, results);

        HashSet<Collider2D> newCollisers = new HashSet<Collider2D>();

        for (int i = 0; i < count; i++)
        {
            var other = results[i];
            if (other == null && other.gameObject == _interactableItem) continue;

            newCollisers.Add(other);

            if (!_currentColliisions.Contains(other))
            {
                ICollision enter = _interactionService.GetCollisionEnterResolve(_interactableItem.itemType, other);
                if (enter == null) continue;
                var result = enter.Execute(_interactableItem);

                ProcessCollisionResult(result, _interactableItem, other);
            }
        }
        foreach (var old in _currentColliisions)
        {
            if (!newCollisers.Contains(old))
            {
                ICollision exit = _interactionService.GetCollisionExitResolve(_interactableItem.itemType);
                if (exit == null) continue;
                var result = exit.Execute(_interactableItem);

                ProcessCollisionResult(result, _interactableItem, old);
            }
        }
        _currentColliisions = newCollisers;
    }
    private void ResetCollisions()
    {
        foreach (var other in _currentColliisions)
        {
            ICollision exit = _interactionService.GetCollisionExitResolve(_interactableItem.itemType);
            if (exit == null) continue;
            var result = exit.Execute(_interactableItem);

            ProcessCollisionResult(result, _interactableItem, other);
        }
    }
}