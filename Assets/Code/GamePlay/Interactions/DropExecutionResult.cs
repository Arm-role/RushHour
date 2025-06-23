using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropExecutionResult
{
    public bool ShouldDestroySelf { get; set; } = false;
    public Action<InteractableItem> SourceInteraction { get; set; }
    public Action<Collider2D> TargetInteraction { get; set; }
    public Action<DragManager> DragInteraction { get; set; }

    public string ParticleToPlay { get; set; } = null;
}
