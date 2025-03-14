using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenWare : MonoBehaviour
{
    [SerializeField]
    private Transform child;

    private List<GameObject> foodObjectOnWare = new List<GameObject>();
    private List<Item> foodOnWare = new List<Item>();

    private Item foodLalest;
    public delegate void KeepFood(GameObject gameObject);
    public event KeepFood OnKeepFood;

    private ItemRendering itemRendering;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private float Time_Origin;

    private float _timer;
    private float Timer
    {
        get
        {
            return _timer;
        }
        set
        {
            _timer = value;
            slider.value = _timer;
        }
    }
    private void Start()
    {
        itemRendering = GetComponent<ItemRendering>();
        EvenManager.NextFoodRequest += SetTime;
        slider.maxValue = Time_Origin;
        Timer = Time_Origin;
    }
    private void Update()
    {
        if(EvenManager.IsReady)
        {
            Timer -= Time.deltaTime;

            if (0.1 >= Timer)
            {
                EvenManager.OnTimeOut();
                Timer = Time_Origin;
            }
        }
        
    }
    private void SetTime()
    {
        Timer = Time_Origin;
    }
    private void CreateOnPlate(GameObject OB)
    {
        GameObject ob = Instantiate(OB, child.position, child.rotation, child);

        float randomZ = Random.Range(0f, 360f);

        ob.transform.rotation = Quaternion.Euler(
        ob.transform.rotation.eulerAngles.x,
        ob.transform.rotation.eulerAngles.y,
        randomZ);

        var renderer = ob.GetComponent<SpriteRenderer>();
        var rendererRef = OB.GetComponent<SpriteRenderer>();

        renderer.sortingOrder = rendererRef.sortingOrder + 1;

        itemRendering.AddRenderor(renderer);
        itemRendering.AddSortingOrder(rendererRef.sortingOrder + 1);

        foodObjectOnWare.Add(ob);
    }
    public void GetOb(GameObject ob)
    {
        Transform tex = ob.transform.GetChild(0);

        var itemHandle = ob.GetComponent<ItemHandle>();
        Item item = itemHandle.Item;
        foodLalest = item;
        foodOnWare.Add(item);

        CreateOnPlate(tex.gameObject);
        OnKeepFood?.Invoke(ob);

        Destroy(ob);
        EvenManager.OnSentToCounter(foodLalest);
    }
    public void ClearIntregients()
    {
        foreach (GameObject item in foodObjectOnWare)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        foodObjectOnWare.Clear();
        foodOnWare.Clear();
    }

    public List<Item> GetFoodOnWare()
    {
        return foodOnWare;
    }
}

