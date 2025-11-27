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

    private int layerDragable;
    private int layerInteractable;

    private readonly Dictionary<int, int> LayerPriority = new();

    private void Awake()
    {
        layerDragable = LayerMask.NameToLayer("Dragable");
        layerInteractable = LayerMask.NameToLayer("Interactable");

        LayerPriority[layerDragable] = 100;
        LayerPriority[layerInteractable] = 50;
    }

    private readonly Dictionary<string, int> TagPriority = new()
    {
        { "Food", 100 },
        { "Tool", 90 },
        { "Counter", 80 },
        { "Trash", 60 },
        { "ArrowLeft", 50 },
        { "ArrowRight", 40 },
    };


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

        if (result.SFXPlay != null)
        {
            EventManager.Invoke(new PlaySFXSound(result.SFXPlay, sourceObject.transform.position));
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

        if (result.SFXPlay != null)
        {
            EventManager.Invoke(new PlaySFXSound(result.SFXPlay, sourceObject.transform.position));
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
        return colliders.Length > 0 ? colliders[0] : null;
    }
    private Collider2D[] GetAllCollidersAtTouchPoint()
    {
        Vector2 touchPos = GetTouchPos();
        var colliders = Physics2D.OverlapPointAll(touchPos);

        if (colliders.Length <= 1)
            return colliders;

        Array.Sort(colliders, (a, b) =>
        {
            int layerA = a.gameObject.layer;
            int layerB = b.gameObject.layer;

            int priA = LayerPriority.TryGetValue(layerA, out var pa) ? pa : 0;
            int priB = LayerPriority.TryGetValue(layerB, out var pb) ? pb : 0;

            // 1) Compare LayerPriority
            int compare = priB.CompareTo(priA);
            if (compare != 0)
                return compare;

            // 2) Compare TagPriority
            string tagA = a.tag;
            string tagB = b.tag;

            int tagPriA = TagPriority.TryGetValue(tagA, out var tpa) ? tpa : 0;
            int tagPriB = TagPriority.TryGetValue(tagB, out var tpb) ? tpb : 0;

            compare = tagPriB.CompareTo(tagPriA);
            if (compare != 0)
                return compare;

            // 3) sortingOrder (สูงสุดอยู่หน้า)
            var srA = a.GetComponent<SpriteRenderer>();
            var srB = b.GetComponent<SpriteRenderer>();
            int orderA = srA != null ? srA.sortingOrder : 0;
            int orderB = srB != null ? srB.sortingOrder : 0;

            compare = orderB.CompareTo(orderA);
            if (compare != 0)
                return compare;

            // 4) fallback: Z (สูงสุดอยู่หน้า)
            return b.transform.position.z.CompareTo(a.transform.position.z);
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