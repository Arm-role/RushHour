using System.Collections.Generic;
using System;

[Serializable]
public class ItemContainerData : StationDataComponent
{
    public Stack<FoodData> FoodContainer = new();
    public int Capacity = 5;

    public override void DebugListeners()
    {
    }

}
