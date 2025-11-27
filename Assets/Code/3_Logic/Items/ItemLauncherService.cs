using UnityEngine;

public class ItemLauncherService
{
    public void Launch(GameObject instance, float ForcePower, Vector2 spawnPoint)
    {
        instance.transform.position = spawnPoint;
        var rigid = instance.GetComponent<Rigidbody2D>();

        Vector2 localPoint = Vector2.up;
        rigid.AddForce(localPoint * ForcePower, ForceMode2D.Impulse);
    }
}
