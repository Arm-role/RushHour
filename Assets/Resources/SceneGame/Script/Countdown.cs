using TMPro;
using UnityEngine;
using UnityEngine.Audio;
public interface ICountdown
{
    void Enter(Countdown countdown);
    void Execute(Countdown countdown);
}

public class Countdown : MonoBehaviour
{
    public GameObject StarUI;
    public TextMeshProUGUI m_TextMeshPro;

    public float timeScala;

    public AudioSource audioSource;
    private bool _isReady;
    public bool IsReady
    {
        get
        {
            return _isReady;
        }
        set
        {
            _isReady = value;
            if (value)
            {
                EvenManager.OnReady(value);
            }
        }
    }

    private ICountdown _countdown;

    void Start()
    {
        audioSource = SoundManager.instance.GetComponent<AudioSource>();
        audioSource.clip = SoundManager.instance.soundClips["countdown"];
        audioSource.volume = 0.8f;
        StarUI.SetActive(false);
        IsReady = false;

        StartCoundown(new StartCoundown());
    }
    void Update()
    {
        _countdown?.Execute(this);
    }
    public void StartCoundown(ICountdown countdown)
    {
        _countdown = countdown;
        _countdown?.Enter(this);
    }
}
public class StartCoundown : ICountdown
{
    public void Enter(Countdown countdown)
    {
        countdown.StarUI.SetActive(true);
        countdown.audioSource.Play();
    }

    public void Execute(Countdown countdown)
    {
        float audioTime = countdown.audioSource.time;

        if (audioTime >= 0 && audioTime < 1)
        {
            countdown.m_TextMeshPro.text = "Ready...";
        }
        else if (audioTime >= 1 && audioTime < 2)
        {
            countdown.m_TextMeshPro.text = "Set...";
        }
        else if (audioTime >= 2 && audioTime < 3)
        {
            countdown.m_TextMeshPro.text = "Go!";
        }
        else if (audioTime >= 3)
        {
            countdown.StarUI.SetActive(false);
            countdown.m_TextMeshPro.text = "Start!";
            countdown.IsReady = true;
            SoundManager.instance.PlayLoopClip("gameScene");
            countdown.StartCoundown(null);
        }
    }
}