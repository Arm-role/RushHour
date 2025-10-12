using UnityEngine;

[CreateAssetMenu(fileName = "newOrder", menuName = "CreateItem/newOrder")]
public class OrderData : Item
{
    public override EItemType itemType => EItemType.Order;
    public Sprite sprite2;
}
