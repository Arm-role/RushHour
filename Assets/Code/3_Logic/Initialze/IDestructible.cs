using System;
using UnityEngine;

public interface IDestructible
{
    event Action<InteractableItem> OnRequestDestruction;
    void RequestDestruction();
}