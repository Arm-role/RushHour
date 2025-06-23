using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newWare", menuName = "CreateItem/newWare")]
public class WareData : Item
{
    public override EItemType itemType => EItemType.Ware;
}
