using UnityEngine;
using UnityEngine.UI;

public class KitchenTool : MonoBehaviour
{
    public Transform child;

    public float Timer;

    public Slider slider;

    public (Item, Item, float TimeLimit) foodItem;

    [HideInInspector]
    public bool isWorking = false;
    [HideInInspector]
    public GameObject ObjectSprite = null;
    [HideInInspector]
    public GameObject SoundOB = null;

    [SerializeField]
    private ToolType toolType;

    private IKitchenTool currentTool;

    public delegate void AddRenderer(SpriteRenderer renderer);
    public AddRenderer addRenderer;

    public delegate void RemoveRenderer(SpriteRenderer renderer);
    public RemoveRenderer removeRenderer;
    private void Start()
    {
        Timer = Time.deltaTime;

        slider.minValue = 0;
        slider.transform.gameObject.SetActive(false);

        switch (toolType)
        {
            case ToolType.ToolFried:
                currentTool = new ToolFriedState();
                break;
            case ToolType.ToolCutted:
                currentTool = new ToolCuttedState();
                break;
        }

    }
    private void Update()
    {
        currentTool?.Execute(this);
    }
    public void GetOb(GameObject ob)
    {
        if (!isWorking)
        {
            Item item = ob.GetComponent<ItemHandle>().Item;

            foreach (FoodState foodState in item.foodState)
            {
                if (foodState.ToolType == toolType)
                {
                    foodItem.Item1 = Instantiate(item);
                    foodItem.Item2 = foodState.item;
                    foodItem.TimeLimit = foodState.Timer;

                    Destroy(ob);
                    isWorking = true;
                    return;
                }
            }
        }
    }
}
