using System.Collections.Generic;

public class OrderRequirementData : StationDataComponent
{
    public int MenuId;
    public List<Item> RequestItems;
    public List<List<Item>> RequiredItemAndCounts;
    public int ScoreValue;
    public float TimeLimit;

    public override void DebugListeners()
    {
    }
}