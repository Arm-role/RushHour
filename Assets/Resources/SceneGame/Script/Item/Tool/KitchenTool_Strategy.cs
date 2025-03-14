
using UnityEngine;
public interface IKitchenTool
{
    void Enter(KitchenTool tool);
    void Execute(KitchenTool tool);
    void Exit(KitchenTool tool);
}

[System.Serializable]
public struct FoodState
{
    public ToolType ToolType;
    public float Timer;
    public Item item;
}
public enum ToolType
{
    ToolFried, ToolCutted
}
public abstract class ToolBaseState : IKitchenTool
{
    public virtual void Enter(KitchenTool tool) { }
    public virtual void Execute(KitchenTool tool)
    {
        if (tool.foodItem.Item2 != null && tool.isWorking)
        {
            tool.slider.value = tool.Timer;

            if (tool.child.childCount < 1)
            {
                SetupObjectSprite(tool);

                tool.slider.maxValue = tool.foodItem.TimeLimit;
                tool.slider.transform.gameObject.SetActive(true);

                OnStateStart(tool);
            }

            if (tool.foodItem.TimeLimit < tool.Timer)
            {
                CompleteTask(tool);
            }
        }
    }
    public virtual void Exit(KitchenTool tool) { }
    protected virtual void SetupObjectSprite(KitchenTool tool)
    {
        GameObject prefab = tool.foodItem.Item1.prefab;
        GameObject texture = prefab.transform.GetChild(0).gameObject;
        tool.ObjectSprite = Object.Instantiate(texture, tool.child.position, tool.child.rotation, tool.child);

        SpriteRenderer renderer = tool.ObjectSprite.GetComponent<SpriteRenderer>();
        SpriteRenderer ThisRenderer = tool.transform.GetChild(0).GetComponent<SpriteRenderer>();
        renderer.sortingOrder = ThisRenderer.sortingOrder + 1;
        tool.addRenderer?.Invoke(renderer);
    }
    protected virtual void OnStateStart(KitchenTool tool) { }
    protected virtual void CompleteTask(KitchenTool tool)
    {
        tool.Timer = 0;

        SpriteRenderer renderer = tool.ObjectSprite.GetComponent<SpriteRenderer>();
        tool.removeRenderer?.Invoke(renderer);
        MonoBehaviour.Destroy(tool.ObjectSprite);

        tool.slider.transform.gameObject.SetActive(false);
        tool.isWorking = false;

        SpawnManager.Instance.OnSpawnItem(tool.foodItem.Item2, tool.transform.position);
        OnTaskComplete(tool);
    }
    protected virtual void OnTaskComplete(KitchenTool tool) { }
}


public class ToolFriedState : ToolBaseState
{
    public override void Execute(KitchenTool tool)
    {
        base.Execute(tool);
        if (tool.isWorking)
        {
            tool.Timer += Time.deltaTime;
        }
    }
    protected override void OnStateStart(KitchenTool tool)
    {
        tool.SoundOB = SoundManager.instance.CreatePersistentSound("frying");
    }
    protected override void OnTaskComplete(KitchenTool tool)
    {
        MonoBehaviour.Destroy(tool.SoundOB);
        ParticleManager.instance.CreateParticle("SmokeParticle", tool.transform.position);
    }
}


public class ToolCuttedState : ToolBaseState
{
    public override void Execute(KitchenTool tool)
    {
        base.Execute(tool);
        if (tool.isWorking && InputHandle.GetDoubleTouch())
        {
            OnDoubleTap(tool);
        }
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