using Fusion;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ReadySystem
{
    public bool Initialize(Dictionary<PlayerRef, bool> _playerReady, Dictionary<PlayerRef, PlayerLobbyData> playerLobby)
    {
        foreach (var item in playerLobby.Values)
        {
            //Debug.Log(_playerReady[item.player]);

            if (item.player != null && _playerReady.TryGetValue(item.player, out bool isReady))
            {
                item.GetReady(isReady);
            }
        }
        if (_playerReady.Values.All(ready => ready))
        {
            return true;
        }
        return false;
    }
}

public class TimerSystem
{
    public IEnumerator TimeDelayCoroutine(TextMeshProUGUI textMesh, float startNum, float endNum, float NotifyNum, Action OnNotify, Action OnEnd)
    {
        bool isPositive = (startNum - endNum) > 0;
        bool hasNotified = false;

        while (isPositive ? startNum > endNum : startNum < endNum)
        {
            if (!hasNotified && Mathf.FloorToInt(startNum) == NotifyNum)
            {
                hasNotified = true;
                OnNotify?.Invoke();
            }
            startNum += (isPositive) ? -Time.deltaTime : Time.deltaTime;

            int Intimer = Mathf.FloorToInt(startNum);
            textMesh.text = Intimer.ToString();
            yield return null;
        }

        OnEnd?.Invoke();
    }
}