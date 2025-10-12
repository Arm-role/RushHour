using UnityEngine;

public class OverEmpty : IDrop
{
    public DropExecutionResult Execute(InteractableItem intercatableItem)
    {
        var result = new DropExecutionResult();
        var state = new ProcessState<Collider2D, Collider2D>();

        state.Act = (source, target) =>
        {
            if (source != null && target != null)
            {
                Physics2D.IgnoreCollision(source, target, false);
            }
        };
        result.SourceInteraction = (source) =>
        {
            if (source.LastTarget != null)
            {
                state.Source = source.GetComponent<Collider2D>();
                state.Target = source.LastTarget.GetComponent<Collider2D>();

                state.Act.Invoke(state.Source, state.Target);
            }
        };
        return result;
    }
}