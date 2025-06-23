using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    
    private float _score;
    public float Score
    {
        get => _score;
        set
        {
            _score += value;
            Debug.Log(_score + " " + value);
            ScoreText.text = _score.ToString();
        }
    }
    void Start()
    {
        Score = 0;
        //PlayerEvents.Instance.OnSentScore.Subscribe(SetScore);
    }
    //private void OnDestroy() => PlayerEvents.Instance.OnSentScore.UnSubscribe(SetScore);
    private void SetScore(float score) => Score = score;
}
