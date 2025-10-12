using UnityEngine;

public interface ICollisionType
{
    ICollision EnterResolve(Collider2D collider);
    ICollision ExitResolve();

}