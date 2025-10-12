using System.Threading.Tasks;
using UnityEngine;

public abstract class InteractionStrategy : ScriptableObject, IInteractionStrategy
{
    public abstract int GetExecutionPriority(InteractableItem source, Station targetStation);
    public abstract Task<bool> Execute(InteractableItem source, Station targetStation);
}