
using UnityEngine;

public class ItemEvents : SingletonLazy<ItemEvents>
{
    public EventBase<Item> OnSentItem { get; private set; } = new SentItemEnvent();
    public EventBase<Menu> OnSentMenu { get; private set; } = new SentMenuEvent();
    public EventBase<Item> OnSentToCounter { get; set; } = new SentItemToCounter();
    public EventBase<Menu> OnMenuFinished { get; private set; } = new MenuFinished();


    public EventBase<bool> OnFoodDisplay { get; set; } = new FoodSwitchDisplay();
    public EventBase OnNextFood { get; set; } = new NextFoodEvent();
    public EventBase<(Sprite, int, int)> OnNextIngredient { get; set; } = new NextIngredient();
    public void OnNextIngredientDisplay(Sprite sp, int amount, int count)
    {
        OnNextIngredient.Invoke((sp, amount, count));
        OnNextFood.Invoke();
    }

    public EventBase OnTimeOut { get; private set; } = new TimeOutEvent();
}

public class SentItemEnvent : EventBase<Item> { }
public class SentMenuEvent : EventBase<Menu> { }
public class SentItemToCounter : EventBase<Item> { }
public class MenuFinished : EventBase<Menu> { }
public class FoodSwitchDisplay : EventBase<bool> { }
public class NextFoodEvent : EventBase { }
public class NextIngredient : EventBase<(Sprite, int, int)> { }

public class TimeOutEvent : EventBase { }