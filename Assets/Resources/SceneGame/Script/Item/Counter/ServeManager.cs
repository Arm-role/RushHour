using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServeManager : MonoBehaviour
{
    public event Action Serve;
    public void AddToServe(Action action)
    {
        if (!Serve.GetInvocationList().Contains(action))
        {
            Serve += action;
        }
    }
    public  void OnServe()
    {
        Serve?.Invoke();
    }

    public event Action<List<Item>> FoodOnWare;
    public void SetFoodOnWare(List<Item> food)
    {
        FoodOnWare?.Invoke(food);
    }
}
