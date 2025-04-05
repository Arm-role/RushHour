using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameLevel
{
    public int Level;
    public Menu[] Menus;
}

public class GameManager : StateMachine<GameManager>
{
    public string BasePath;
    public List<Menu> LevelLst;

    [HideInInspector]
    public int currentIndex;
    public bool IsRandomIndex = false;
    public bool RandomItemBetweenPlayer;

    public bool isTutorial = false;

    private void Start()
    {
        ItemEvents.Instance.OnMenuFinished.Subscribe(GetMenuFinished);
        currentIndex = UnityEngine.Random.Range(0, LevelLst.Count);
        SetState(new GameManager_Setup());
    }
    private void OnDestroy()
    {
        ItemEvents.Instance.OnMenuFinished.UnSubscribe(GetMenuFinished);
    }

    private void Update() { Execute(); }


    public void RunMenu(int menuIndex)
    {
        if (menuIndex >= LevelLst.Count)
        {
            Debug.Log("No more menus to run or index out of range.");
            IsRandomIndex = true;
            currentIndex = UnityEngine.Random.Range(0, LevelLst.Count);
        }
        else
        {
            currentIndex = (IsRandomIndex) ? UnityEngine.Random.Range(0, LevelLst.Count) : menuIndex; // ✅ อัปเดต index ปัจจุบัน
        }

        Menu menu = LevelLst[currentIndex];

        if (RandomItemBetweenPlayer)
        {
            ConnectorManager.TranferToPlayer(menu);
        }
        ItemEvents.Instance.OnSentMenu.Invoke(menu);
    }
    public void GetMenuFinished(Menu menu)
    {
        if (menu == LevelLst[currentIndex])
        {
            currentIndex++;
        }
        else
        {
            Debug.LogError("Menu Doesn't Match");
        }

        if (currentIndex < LevelLst.Count)
        {
            RunMenu(currentIndex);
        }
        else
        {
            SetState(new Wait_For_NextLevel());
            Debug.Log($"All menus completed!");
        }
    }
}
