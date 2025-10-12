using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ModeData", menuName = "Game/ModeData")]
public class GameModeData : ScriptableObject
{
    public int GameModeID;
    public string ModeName;

    public bool LevelLoop;

    public int RandomLevelMin;
    public int RandomLevelMax;

    public List<LevelData> LevelList;

    public LevelData GetRandomLevel()
    {
        if(LevelList == null || LevelList.Count < RandomLevelMin || LevelList.Count < RandomLevelMax) return null;

        int levelRandom = UnityEngine.Random.Range(RandomLevelMin, RandomLevelMax);
        return LevelList[levelRandom];
    }
}