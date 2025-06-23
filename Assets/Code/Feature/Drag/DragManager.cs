using System;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using WebSocketSharp;

public class DragManager : StateMachine<DragManager>
{
    private GameObject _currentItem;
    private Rigidbody2D _currentRB;
    private InteractableItem _interactableItem;

    private ParticleManager _particleManager;

    private HashSet<Collider2D> _currentColliisions = new();

    private InteractionService _interactionService = new();

    public GameObject currentItem => _currentItem;

    #region dragState

    [HideInInspector] public float speed = 60;
    public float holdThreshold = 0.5f;
    public float holdTimer = 0f;

    public Vector2 lastTouchPos;
    public bool hasMovedTooMuch = false;
    [HideInInspector] public float holdMoveTolerance = 0.5f;

    public bool isTast = false;

    #endregion

    private void Start()
    {
        SetDragState();
        SetState(new Idle_DragState());
    }

    #region Process

    private void Update()
    {
        if (isTast)
        {
            Execute();
        }
    }

    public void Initialze(ParticleManager particleManager)
    {
        _particleManager = particleManager;
    }

    public void SetItem(Collider2D hitColl)
    {
        _currentItem = hitColl.gameObject;
        _currentItem.TryGetComponent(out _currentRB);
        _currentItem.TryGetComponent(out _interactableItem);
    }
    public void ClearItem()
    {
        _currentItem = null;
        _currentRB = null;
        _interactableItem = null;
    }
    public Vector2 GetTouchPos()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(InputHandle.GetTouchPosition());
        return new Vector2(mouseWorldPos.x, mouseWorldPos.y);
    }

    #endregion

    #region Action Drag

    private Dictionary<Type, Action> _dragStateEnter;
    private Dictionary<Type, Action> _dragStateExcute;
    private Dictionary<Type, Action> _dragStateExit;
    private void SetDragState()
    {
        _dragStateEnter = new Dictionary<Type, Action>
        {
            {typeof(Grabbed_DragState),() => _interactableItem.SetOrderFront() },
            {typeof(Release_DragState), () => _interactableItem.ResetOrder() }
        };
        _dragStateExcute = new Dictionary<Type, Action>
        {
            {typeof(Move_DragState),() => OnMoveExcute() },
            {typeof(Dropped_DragState), () => OnDropExcute()}
        };
        _dragStateExit = new Dictionary<Type, Action>
        {
            {typeof(Release_DragState),() => OnReleaseExit() },
        };
    }
    #region NotFix DragNotify
    public void EnterNotify(IDrag drag)
    {
        if (_dragStateEnter.TryGetValue(drag.GetType(), out Action action)) { action(); }
    }
    public void ExcuteNotify(IDrag drag)
    {
        if (_dragStateExcute.TryGetValue(drag.GetType(), out Action action)) { action(); }
    }
    public void ExitNotify(IDrag drag)
    {
        if (_dragStateExit.TryGetValue(drag.GetType(), out Action action)) { action(); }
    }
    #endregion
    private void OnMoveExcute()
    {
        Vector2 dir = Vector2.MoveTowards(_currentRB.position, GetTouchPos(), speed * Time.fixedDeltaTime);
        _currentRB.MovePosition(dir);

        var coll = currentItem.GetComponent<Collider2D>();
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
    private void OnDropExcute()
    {
        Collider2D[] hit = Physics2D.OverlapPointAll(GetTouchPos());

        foreach (Collider2D collider in hit)
        {
            if (collider != null && collider.gameObject != _currentItem)
            {
                IDrop drop = _interactionService.GetDropResolve(_interactableItem.itemType, collider);
                if (drop == null) continue;
                var result = drop.Execute(_interactableItem);

                ProcessDropResult(result, _interactableItem, collider);
            }
        }
    }
    private void OnReleaseExit()
    {
        foreach (var other in _currentColliisions)
        {
            ICollision exit = _interactionService.GetCollisionExitResolve(_interactableItem.itemType);
            if (exit == null) continue;
            var result = exit.Execute(_interactableItem);

            ProcessCollisionResult(result, _interactableItem, other);
        }
    }



    #endregion
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
    private void ProcessDropResult(DropExecutionResult result, InteractableItem sourceObject, Collider2D targetCollider)
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

        if (result.ParticleToPlay != null)
        {
            _particleManager.Play(result.ParticleToPlay, sourceObject.transform.position);
        }

        if (result.ShouldDestroySelf)
        {
            sourceObject.RequestDestruction();
        }
    }
}