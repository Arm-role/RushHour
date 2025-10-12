using UnityEngine;

public class ItemLauncherService
{
    public void Launch(GameObject instance, float ForcePower, Transform spawnPoint)
    {
        instance.transform.position = spawnPoint.position;
        var rigid = instance.GetComponent<Rigidbody2D>();

        Vector2 localPoint = spawnPoint.up;
        rigid.AddForce(localPoint * ForcePower, ForceMode2D.Impulse);
    }
}
