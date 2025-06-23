using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToolView : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private StationEvents _stationEvent;
    public void Initialze(StationEvents stationEvent)
    {
        _stationEvent = stationEvent;

        _stationEvent.OnWorkStarted.Subscribe(OnStart);
        _stationEvent.OnWorkPogress.Subscribe(OnProgress);
        _stationEvent.OnWorkCompleted.Subscribe(OnComplete);
    }
    private void OnDisable()
    {
        _stationEvent.OnWorkStarted.UnSubscribe(OnStart);
        _stationEvent.OnWorkPogress.UnSubscribe(OnProgress);
        _stationEvent.OnWorkCompleted.UnSubscribe(OnComplete);
    }
    private void OnStart(Station station)
    {
        slider.maxValue = station.stationData.MaxTime;
        spriteRenderer.sprite = station.stationData.RawFood.sprite;

        slider.gameObject.SetActive(true);
        spriteRenderer.enabled = true;

        Debug.Log("Start");
    }
    private void OnProgress(Station station)
    {
        slider.value = station.stationData.CurrentTime;
        //Debug.Log($"Progress {station.stationData.CurrentTime}");
    }
    private void OnComplete(Station station)
    {
        slider.gameObject.SetActive(false);
        spriteRenderer.enabled = false;

        Debug.Log("Complete");
    }
}