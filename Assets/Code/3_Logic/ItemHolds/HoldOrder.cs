using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldOrder : IHold
{
    public HoldExecutionResult Execute(InteractableItem interactableItem)
    {
        var result = new HoldExecutionResult()
        {
            SourceInteraction = async (source) =>
            {
                if (source.TryGetComponent<Station>(out var station))
                {
                    await station.Interact();
                }
            }
        };

        return result;
    }
}
