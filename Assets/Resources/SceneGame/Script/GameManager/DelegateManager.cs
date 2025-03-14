using System;
using UnityEngine;

public class EvenManager
{
    #region GameStartEnd

    public static event Action<ICountdown> Coundown;
    public static void OnCoundown(ICountdown coundown)
    {
        Coundown?.Invoke(coundown);
    } 

    public static bool IsReady { get; set; }
    public static event Action<bool> Ready;
    public static void OnReady(bool isReady)
    {
        IsReady = isReady;
        Ready?.Invoke(isReady);
    }

    public static bool IsEnd { get; set; }
    public static event Action<bool> End;
    public static void OnEnd(bool isEnd)
    {
        IsEnd = isEnd;
        IsReady = !isEnd;
        End?.Invoke(isEnd);
    }

    #endregion


    public static event Action<bool> OnTouchItemChanged;
    public static void OnTouchItem(bool isTouch)
    {
        OnTouchItemChanged?.Invoke(isTouch);
    }
    public static event Action<bool> ArrowTouchItemChanged;
    public static void OnArrowTouchItem(bool isTouch)
    {
        ArrowTouchItemChanged?.Invoke(isTouch);
    }

    public static event Action<Item> OnSentToCounterChanged;
    public static void OnSentToCounter(Item food)
    {
        OnSentToCounterChanged?.Invoke(food);
    }

    public static event Action<bool> foodStateDisplay;
    public static void OnfoodStateDisplay(bool isTouch)
    {
        foodStateDisplay?.Invoke(isTouch);
    }

    public static event Action NextFoodRequest;
    public static event Action<Sprite, int, int> NextIngredient;
    public static void OnNextIngredient(Sprite sp, int amount, int count)
    {
        NextIngredient?.Invoke(sp, amount, count);
        NextFoodRequest?.Invoke();
    }

    #region Player

    public static float TotalScore { get; set; } = 0;
    public static event Action<float> SentTotalScore;
    public static void OnSentTotalScore(float score)
    {
        TotalScore += score;
        SentTotalScore?.Invoke(score);
    }

    public static event Action<float> TimeSpeed;
    public static void OnTimeSpeed(float score)
    {
        TimeSpeed?.Invoke(score);
    }

    #endregion




    #region Counter

    public static float Score { get; set; }
    public static event Action<float> SentScore;
    public static void OnSentScore(float score)
    {
        Score += score;
        SentScore?.Invoke(score);
    }

    public static event Action TimeOut;
    public static void OnTimeOut()
    {
        TimeOut?.Invoke();
    }

    #endregion




    #region Spawner

    public static event Action<Item> SentItem;
    public static void OnSentItem(Item item)
    {
        SentItem?.Invoke(item);
    }
    #endregion




    #region GameManager

    public static bool IsRandomItem { get; set; }
    public static event Action<bool> RandomItem;
    public static void OnRandomItem(bool randomItem)
    {
        IsRandomItem = randomItem;
        RandomItem?.Invoke(randomItem);
    }

    public static event Action<Menu> SentMenu;
    public static void OnSentMenu(Menu menu)
    {
        SentMenu?.Invoke(menu);
    }

    public static event Action<Menu> MenuFinished;
    public static void OnMenuFinished(Menu menu)
    {
        MenuFinished?.Invoke(menu);
    }

    public static event Action<int> EndCurrentLevel;
    public static void OnEndCurrentLevel(int level)
    {
        EndCurrentLevel?.Invoke(level);
    }

    #endregion
}
