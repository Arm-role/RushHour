using UnityEngine;

public class Idle_DragState : IDrag //อยู่นิ่งๆ
{
    public InteractionResult OnEnter()
    {
        return new InteractionResult(shouldClearItem: true);
    }

    public StateExecutionResult OnExecute(DragContext context)
    {
        if (context.IsTouch)
        {
            if (context.HitCollider != null && context.HitCollider.gameObject.layer == 3)
            {
                var hitCollider = FindSorce(context.HitColliders);
                var interaction = InteractionResult.SetItem(hitCollider);
                return StateExecutionResult.TransitionWithInteraction(new Grabbed_DragState(), interaction);
            }
        }
        return StateExecutionResult.DoNothing();
    }

    public InteractionResult OnExit()
    {
        return null;
    }

    private Collider2D FindSorce(Collider2D[] colliders)
    {
        if (colliders == null) return null;

        foreach (var col in colliders)
        {
            if (col.gameObject.layer == 3)
            {
                return col;
            }
        }

        return null;
    }
}
