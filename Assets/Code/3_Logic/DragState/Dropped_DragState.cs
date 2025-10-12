using UnityEngine;

public class Dropped_DragState : IDrag // ตกใส่เป้าหมาย
{
    public InteractionResult OnEnter()
    {
        return null;
    }

    public StateExecutionResult OnExecute(DragContext context)
    {
        var target = FindBestTarget(context.HitColliders, context.DraggedItem);

        if (target != null)
        {
            InteractionResult dropResult = new InteractionResult(targetCollider: target);
            return StateExecutionResult.TransitionWithInteraction(new Idle_DragState(), dropResult);
        }

        return StateExecutionResult.TransitionTo(new Idle_DragState());
    }
    public InteractionResult OnExit()
    {
        return null;
    }
    private Collider2D FindBestTarget(Collider2D[] colliders, InteractableItem source)
    {
        if (colliders == null || source == null) return null;

        foreach (var col in colliders)
        {
            if (col.gameObject == source.gameObject) continue;

            return col;
        }
        return null;
    }
}
