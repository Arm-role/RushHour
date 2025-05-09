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

    [SerializeField]
    private float gameTimeChanged;
    [SerializeField]
    private float gameSpeedChanged;

    private bool isGameSpeedChanged = false;
    void Start()
    {
        GameEvents.Instance.OnSentTotalScore.Subscribe(GetTimer);
        PlayerEvents.Instance.TimeSpeed.Subscribe(SetTimeSpeed);

        Timer = _Timer;
        UIEnd.SetActive(false);
    }
    private void OnDestroy()
    {
        GameEvents.Instance.OnSentTotalScore.UnSubscribe(GetTimer);
        PlayerEvents.Instance.TimeSpeed.UnSubscribe(SetTimeSpeed);
    }
    private void Update()
    {
        if (GameEvents.Instance.IsGameRun)
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
    public void GetTimer(float timer)
    {
        Timer = MaxValue;
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
        GameEvents.Instance.OnGameState.Invoke(EGameState.End);
    }
}
