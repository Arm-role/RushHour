using System;
using UnityEngine;

public class HoldExecutionResult
{
    public bool ShouldDestroySelf { get; set; } = false;
    public Action<InteractableItem> SourceInteraction { get; set; }
    public string ParticleToPlay { get; set; } = null;
}