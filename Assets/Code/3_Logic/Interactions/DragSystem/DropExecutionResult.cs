using System;
using System.Threading.Tasks;
using UnityEngine;

public class DropExecutionResult
{
    public Task<bool> ShouldDestroySelf { get; set; } = Task.FromResult(false);
    public Task<bool> ShouldDestroyTarget { get; set; } = Task.FromResult(false);
    public Action<InteractableItem> SourceInteraction { get; set; }
    public Action<Collider2D> TargetInteraction { get; set; }
    public string ParticleToPlay { get; set; } = null;
    public string SFXPlay { get; set; } = null;
}
