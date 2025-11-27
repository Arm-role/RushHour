using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

[Serializable]
public class OrderLifecycleManager : StationDataComponent
{
    public IOrderState CurrentState;
    public OrderRequirementData RequirementData { get; private set; }
    public Station OrderStation { get; private set; }
    public Station LinkedPlate { get; set; } = null;

    // สถานะภายในที่ State จะแก้ไข
    public float CurrentItemTime { get; set; }
    public float CurrentTime { get; set; }

    public int CurrentScore { get; set; }
    public List<string> CollectedItems { get; set; } = new();

    // --- Public API ---
    public void Initialize(Station ownerStation, OrderRequirementData data)
    {
        OrderStation = ownerStation;
        RequirementData = data;
        CurrentTime = data.TimeLimit;
        CurrentScore = data.ScoreValue;

        SetState(new OrderState_AwaitingActivation());
    }

    public Task<bool> OnInteract(InteractableItem source) => CurrentState?.HandleInteraction(this, source) ?? Task.FromResult(false);
    public void OnIngredientAddedToPlate(Item ingredient) => CurrentState?.OnIngredientAdded(this, ingredient);
    public void OnIngredientRemoveFromPlate(Item ingredient) => CurrentState?.OnIngredientRemoved(this, ingredient);
    public void Update() => CurrentState?.OnUpdate(this);

    public void SetState(IOrderState newState)
    {
        CurrentState?.OnExit(this);
        CurrentState = newState;
        CurrentState?.OnEnter(this);
    }
    public (int, Item) GetFirstItemRequest()
    {
        foreach (var group in RequirementData.RequiredItemAndCounts)
        {
            foreach (var item in group)
            {
                int collectedCount = CollectedItems.Count(c => c == item.Name);

                int requiredCount = group.Count(i => i == item);

                if (collectedCount < requiredCount)
                {
                    return (requiredCount - collectedCount, item);
                }
            }
        }

        return (0, null);
    }
    public override void DebugListeners()
    {

    }
}
