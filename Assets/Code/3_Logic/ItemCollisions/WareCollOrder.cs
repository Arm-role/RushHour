using UnityEngine;

public class WareCollOrder : ICollision
{
    public CollisionExecutionResult Execute(InteractableItem interactableItem)
    {
        var result = new CollisionExecutionResult();
        var state = new ProcessState<Collider2D, Collider2D>();

        state.Act = (source, target) =>
        {
            if (source != null && target != null)
            {
                Physics2D.IgnoreCollision(source, target, true);
            }
        };

        result.TargetInteraction = (target) =>
        {
            state.Target = target;
            state.Act.Invoke(state.Source, state.Target);
        };
        result.SourceInteraction = (source) =>
        {
            state.Source = source.GetComponent<Collider2D>();
            state.Act.Invoke(state.Source, state.Target);
        };

        return result;
    }
}
