using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int LevelId;
    public string LevelName;
    public int RequestCount;

    public PopupData[] StartPopupSprites;

    public List<Menu> MenuList;

}

[Serializable]
public struct PopupData
{
    public float timer;
    public Sprite StartPopupSprite;
}