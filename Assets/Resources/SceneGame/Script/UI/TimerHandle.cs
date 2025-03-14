using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerHandle : MonoBehaviour
{
    [SerializeField]
    private Slider[] slider;

    private float MaxValue;
    private float _timer;

    [SerializeField]
    private GameObject UIEnd;
    private float Timer
    {
        get
        {
            return _timer;
        }
        set
        {
            _timer = value;
            SetValue(_timer);

            if (_timer > MaxValue)
            {
                SetMaxValue(Timer);
            }
        }
    }
    [SerializeField]
    private float TimeSpeed;

    [SerializeField]
    private float _Timer;
    void Start()
    {
        EvenManager.SentTotalScore += GetTimer;
        EvenManager.TimeSpeed += SetTimeSpeed;
        Timer = _Timer;
        UIEnd.SetActive(false);
    }
    private void Update()
    {
        if (EvenManager.IsReady)
        {
            Timer -= Time.deltaTime * TimeSpeed; 

            if (Timer <= 0.1)
            {
                Timer = 0;
                OnEnd();
            }
        }
    }
    public void GetTimer(float timer)
    {
        Timer += timer;
    }
    public void SetValue(float value)
    {
        foreach (var s in slider)
        {
            s.value = value;
        }
    }
    public void SetMaxValue(float value)
    {
        MaxValue = value;
        foreach (var s in slider)
        {
            s.maxValue = MaxValue;
        }
    }
    public void SetTimeSpeed(float speed)
    {
        TimeSpeed = speed;
    }
    public void OnEnd()
    {
        UIEnd.SetActive(true);
        EvenManager.OnEnd(true);
    }
}
