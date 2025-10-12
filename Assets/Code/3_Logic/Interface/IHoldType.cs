using UnityEngine;

public interface IHoldType
{
    IHold Resolve(Collider2D collider);
}