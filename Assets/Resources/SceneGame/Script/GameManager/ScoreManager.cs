using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;

    private float _score;
    private float Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score += value;
            ScoreText.text = _score.ToString();
        }
    }
    void Start()
    {
        Score = 0;
        EvenManager.SentScore += SetScore;
    }
    private void SetScore(float score)
    {
        Score = score;
    }
}
