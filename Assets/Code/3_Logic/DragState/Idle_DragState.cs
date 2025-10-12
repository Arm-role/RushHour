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
            if (context.HitCollider != null && context.HitCollider.CompareTag("Dragable"))
            {
                var interaction = InteractionResult.SetItem(context.HitCollider);
                return StateExecutionResult.TransitionWithInteraction(new Grabbed_DragState(), interaction);
            }
        }
        return StateExecutionResult.DoNothing();
    }

    public InteractionResult OnExit()
    {
        return null;
    }
}
