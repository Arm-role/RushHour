using UnityEngine;

[System.Serializable]
public struct FoodState
{
    public EToolType ToolType;
    public float Timer;
    public Item item;
}

public abstract class ToolBaseState : IKitchenTool
{
    public virtual void Execute(KitchenTool tool)
    {
        if (tool.isWorking && GameEvents.Instance.IsGameRun)
        {
            if (InputHandle.GetHoldBack() && tool.TouchOverMe(tool.gameObject))
            {
                tool.DropItem();
                OnTaskComplete(tool);
                return;
            }

            tool.slider.value = tool.Timer;

            if (tool.slider.maxValue != tool.TimeCooking)
            {
                tool.slider.maxValue = tool.TimeCooking;
                tool.slider.transform.gameObject.SetActive(true);

                OnStateStart(tool);
            }

            if (tool.TimeCooking < tool.Timer) CompleteTask(tool);
        }
        else if (tool.isWorking && !GameEvents.Instance.IsGameRun)
        {
            FeilTask(tool);
        }
    }
    protected virtual void OnStateStart(KitchenTool tool) { }
    protected virtual void CompleteTask(KitchenTool tool)
    {
        tool.StopWorking();
        Object.Destroy(tool.ObjectSprite);

        SpawnManager.Instance.OnSpawnItem(tool.CookedItem, tool.transform.position);
        OnTaskComplete(tool);
    }
    protected virtual void OnTaskComplete(KitchenTool tool) { }
    protected virtual void FeilTask(KitchenTool tool)
    {
        tool.StopWorking();
        OnTaskFail(tool);
    }
    protected virtual void OnTaskFail(KitchenTool tool) { }
}


public class ToolFriedState : ToolBaseState
{
    public override void Execute(KitchenTool tool)
    {
        base.Execute(tool);
        if (tool.isWorking) tool.Timer += Time.deltaTime;
    }
    protected override void OnStateStart(KitchenTool tool)
    {
        tool.SoundOB = SoundManager.instance.CreatePersistentSound("frying");
    }
    protected override void OnTaskComplete(KitchenTool tool)
    {
        Object.Destroy(tool.SoundOB);
        ParticleManager.instance.CreateParticle("SmokeParticle", tool.transform.position);
    }
    protected override void OnTaskFail(KitchenTool tool)
    {
        Debug.Log(tool.SoundOB != null);
        Object.Destroy(tool.SoundOB);
    }
}

public class ToolCuttedState : ToolBaseState
{
    public override void Execute(KitchenTool tool)
    {
        base.Execute(tool);
        if (tool.isWorking && InputHandle.GetDoubleTouch()) OnDoubleTap(tool);
    }
    private void OnDoubleTap(KitchenTool tool)
    {
        tool.Timer += 1f;
        tool.SoundOB = SoundManager.instance.CreatePersistentSound("cutting");
    }
    protected override void OnTaskComplete(KitchenTool tool)
    {
        ParticleManager.instance.CreateParticle("SmokeParticle", tool.transform.position);
    }
}