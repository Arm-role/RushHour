using System;
using System.Collections.Generic;
using UnityEngine;

public class KitchenTool : KitchenItem
{
    public Item RawItem { get; private set; }
    public Item CookedItem { get; private set; }
    public float TimeCooking { get; private set; }

    public bool isWorking { get; private set; }
    public GameObject ObjectSprite { get; private set; }
    public GameObject SoundOB { get; set; }

    [SerializeField]
    private EToolType toolType;
    private IKitchenTool currentTool;

    protected override void Start()
    {
        base.Start();

        Timer = Time.deltaTime;

        slider.minValue = 0;
        slider.transform.gameObject.SetActive(false);

        switch (toolType)
        {
            case EToolType.ToolFried:
                currentTool = new ToolFriedState();
                break;
            case EToolType.ToolCutted:
                currentTool = new ToolCuttedState();
                break;
        }
    }


    protected override void Update() => currentTool?.Execute(this);
    public void StopWorking()
    {
        slider.transform.gameObject.SetActive(false);
        slider.maxValue = 1;
        isWorking = false;
        Timer = 0;
    }

    public override void PickUpItem(ItemHandle Handle)
    {
        if (!isWorking)
        {
            Item item = Handle.Item;

            foreach (FoodState foodState in item.foodState)
            {
                if (foodState.ToolType == toolType)
                {
                    RawItem = Instantiate(item);
                    CookedItem = foodState.item;
                    TimeCooking = foodState.Timer;

                    GameObject prefab = item.prefab;
                    ObjectSprite = CreateOnPlate(prefab.transform.GetChild(0).gameObject, false);

                    Handle.Self_Destruct();
                    isWorking = true;

                    return;
                }
            }
        }
    }
    public override void DropItem()
    {
        if (RawItem != null && ObjectSprite)
        {
            Destroy(ObjectSprite);
            SpawnManager.Instance.OnSpawnItem(RawItem, transform.position);
            StopWorking();
        }
    }
}