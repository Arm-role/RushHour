using UnityEngine;

public interface IDropType
{
    IDrop Resolve(Collider2D collider);
}
