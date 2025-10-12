using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Move_DragState : IDrag // ลาก
{
    public InteractionResult OnEnter()
    {
        return new InteractionResult(sortOrderType: ESortOrder.Front);
    }

    public StateExecutionResult OnExecute(DragContext context)
    {
        if (context.IsInputReleased)
        {
            return StateExecutionResult.TransitionTo(new Release_DragState());
        }

        if (context.IsInputHold)
        {
            if (!context.HasAlreadyMovedTooMuch &&
                Vector2.Distance(context.CurrentTouchPosition, context.StartTouchPosition) > context.HoldMoveTolerance)
            {
                var update = new DragStateUpdate { NewHasMovedTooMuch = true };
                return StateExecutionResult.TriggerInteraction(new InteractionResult(stateUpdate: update, shouldMoveItem: true, checkCollisions: true));
            }

            if (!context.HasAlreadyMovedTooMuch)
            {
                float newTimer = context.CurrentHoldTimer + context.DeltaTime;
                if (newTimer >= context.HoldThreshold)
                {
                    InteractionResult holdResult = new InteractionResult(isHold: true);
                    return StateExecutionResult.TransitionWithInteraction(new Hold_DragState(), holdResult);
                }
                else
                {
                    var update = new DragStateUpdate { NewHoldTimer = newTimer };
                    return StateExecutionResult.TriggerInteraction(new InteractionResult(stateUpdate: update, shouldMoveItem: true, checkCollisions: true));
                }
            }

            return StateExecutionResult.TriggerInteraction(new InteractionResult(shouldMoveItem: true, checkCollisions: true));
        }

        return StateExecutionResult.DoNothing();
    }

    public InteractionResult OnExit()
    {
        return null;
    }
}
