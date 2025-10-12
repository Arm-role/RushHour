using GameEvents;
using PlayerEvents;
using UnityEngine;
using UnityEngine.UI;

public class TimerHandle_UnLayer : MonoBehaviour
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

    [SerializeField]
    private float gameTimeChanged;
    [SerializeField]
    private float gameSpeedChanged;

    private bool isGameSpeedChanged = false;

    private GameFlowSettup _gameFlow;
    void Start()
    {
        EventManager.Subscribe<TimeSpeed>(SetTimeSpeed);

        Timer = _Timer;
        UIEnd.SetActive(false);
    }
    private void OnDestroy()
    {
        EventManager.Unsubscribe<TimeSpeed>(SetTimeSpeed);
    }

    public void Initialze(GameFlowSettup gameFlow)
    {
        _gameFlow = gameFlow;
    }
    private void Update()
    {
        if (_gameFlow.TryFlow(EGameFlow.Run))
        {
            Timer -= Time.deltaTime * TimeSpeed;
            if (Timer <= 0.1)
            {
                Timer = 0;
                OnEnd();
            }
            if(!isGameSpeedChanged)
            {
                gameTimeChanged -= Time.deltaTime;
            }
            if (gameTimeChanged <= 0.1 && !isGameSpeedChanged)
            {
                TimeSpeed = gameSpeedChanged;
                isGameSpeedChanged = true;
            }
        }
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
    public void SetTimeSpeed(TimeSpeed evt)
    {
        TimeSpeed = evt.Speed;
    }
    public void OnEnd()
    {
        UIEnd.SetActive(true);
        GameFlowState.Set(EGameFlow.End);
    }
}
