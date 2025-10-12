using UnityEngine;

[CreateAssetMenu(fileName = "newTool", menuName = "CreateItem/newTool")]
public class ToolData : Item
{
    public EItemType _itemType = EItemType.Tool;
    public override EItemType itemType => _itemType;
    public EToolType toolType;
}
