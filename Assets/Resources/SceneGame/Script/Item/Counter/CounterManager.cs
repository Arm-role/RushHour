using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CounterManager : MonoBehaviour
{
    [SerializeField]
    private ServeManager serverManager;
    private List<Item> foodOnWare = new List<Item>();

    private Menu currentMenu;
    private ItemAndCount currentItem;
    private List<ItemAndCount> ListFood_Count;

    private List<Item> foodRequest = new List<Item>();

    private int score = 0;
    private int IntrigrintAmmount = 0;

    private bool OutOfStock;
    private void Start()
    {
        EvenManager.SentMenu += LoadMenu;
        EvenManager.OnSentToCounterChanged += ImportedItem;
        EvenManager.TimeOut += TimeOut;

        serverManager.FoodOnWare += GetFoodOnWare;
        serverManager.Serve += Serve;
    }
    private List<Item> FlattenMenu(List<ItemAndCount> menus)
    {
        List<Item> itemList = new List<Item>();
        foreach (ItemAndCount menu in menus)
        {
            for (int i = 0; i < menu.count; i++)
            {
                itemList.Add(menu.item);
            }
        }
        return itemList;
    }
    public void LoadMenu(Menu menu)
    {
        currentMenu = menu;
        ListFood_Count = menu.FoodRequest.ToList();
        foodRequest = FlattenMenu(ListFood_Count);
        score = menu.Score;

        OutOfStock = ListFood_Count.Count > 0;

        SetCurrentItem();
        MenuDisplay();
    }
    private void SetCurrentItem()
    {
        if (ListFood_Count.Count > 0)
        {
            currentItem = ListFood_Count.FirstOrDefault();
        }
        else
        {
            OutOfStock = true;
        }
    }

    private void MenuDisplay()
    {
        if (ListFood_Count.Count > 0)
        {
            EvenManager.OnNextIngredient(currentItem.item.sprite, IntrigrintAmmount, currentItem.count);
        }
        else
        {
            EvenManager.OnfoodStateDisplay(OutOfStock);
        }
    }
    public void ImportedItem(Item item)
    {
        if (item == currentItem.item)
        {
            IntrigrintAmmount++;

            if (currentItem.count == IntrigrintAmmount)
            {
                ListFood_Count.Remove(currentItem);
                SetCurrentItem();
                IntrigrintAmmount = 0;
            }
            MenuDisplay();
        }
    }

    private void GetFoodOnWare(List<Item> items)
    {
        foodOnWare = items;
        serverManager.OnServe();
    }
    private void Serve()
    {
        if (OutOfStock)
        {
            float newScore = CalculateScore();
            EvenManager.OnSentScore(newScore);

            EvenManager.OnMenuFinished(currentMenu);
        }
    }
    private void TimeOut()
    {
        if (OutOfStock) 
        {
            Serve();
        }
        else if (ListFood_Count != null && ListFood_Count.Count > 0)
        {
            ImportedItem(currentItem.item);
        }
    }
    private float CalculateScore()
    {
        if (foodRequest.Count == 0) return 0;
        float totalScore = 0;
        float eachScore = (foodRequest.Count > foodOnWare.Count) ?
            (float)score / foodRequest.Count : (float)score / foodOnWare.Count;

        float eachScoreInt = Mathf.Round(eachScore * 100f) / 100f;

        foreach (Item item in foodRequest)
        {
            if (foodOnWare.Contains(item))
            {
                totalScore += eachScoreInt;
                foodOnWare.Remove(item);
            }
        }

        return totalScore;
    }
}
