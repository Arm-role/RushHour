using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "newFood", menuName = "CreateItem/newFood")]
public class FoodData : Item
{
    public override EItemType itemType => EItemType.Food;

    public FoodState[] foodState;
    
    private Dictionary<EToolType, FoodState> _toolState;
    public Dictionary<EToolType, FoodState> ToolState
    {
        get
        {
            if (_toolState == null)
            {
                _toolState = new Dictionary<EToolType, FoodState>();
                foreach (var state in foodState)
                {
                    if (!_toolState.ContainsKey(state.ToolType))
                    {
                        _toolState[state.ToolType] = state;
                    }
                }
            }
            return _toolState;
        }
    }
    public bool CanCook(EToolType toolType)
    {
        if(ToolState.ContainsKey(toolType)) return true;
        return false;
    }

}
[Serializable]
public class FoodState
{
    public float Timer = 0;
    public EToolType ToolType;
    public AssetReferenceT<FoodData> CookItem;
}