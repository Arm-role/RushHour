using UnityEngine;

public class Release_DragState : IDrag //ปล่อย
{
    public InteractionResult OnEnter()
    {
        return new InteractionResult(sortOrderType: ESortOrder.Reset);
    }

    public StateExecutionResult OnExecute(DragContext context)
    {
        bool foundOther = false;
        foreach (Collider2D collider in context.HitColliders)
        {
            if (collider.gameObject != context.DraggedItem?.gameObject)
            {
                foundOther = true;
                break;
            }
        }

        if (foundOther)
        {
            return StateExecutionResult.TransitionTo(new Dropped_DragState());
        }

        return StateExecutionResult.TransitionTo(new Idle_DragState());
    }

    public InteractionResult OnExit()
    {
        return new InteractionResult(resetCollisions: true);
    }
}
