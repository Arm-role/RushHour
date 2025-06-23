using UnityEngine;

[CreateAssetMenu(fileName = "newTool", menuName = "CreateItem/newTool")]
public class ToolData : Item
{
    public override EItemType itemType => EItemType.Tool;
    public EToolType toolType;
}
