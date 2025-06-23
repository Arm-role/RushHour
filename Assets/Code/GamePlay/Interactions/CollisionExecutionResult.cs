using System;
using UnityEngine;

public class CollisionExecutionResult
{
    public Action<InteractableItem> SourceInteraction { get; set; }
    public Action<Collider2D> TargetInteraction { get; set; }
}