using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int LevelId;
    public string LevelName;
    public int RequestCount;
    public float MaxGameTime;

    public PopupData[] StartPopupSprites;

    public Item[] ItemHelp;
    public List<Menu> MenuList;
}

[Serializable]
public struct PopupData
{
    public float timer;
    public Sprite StartPopupSprite;
}