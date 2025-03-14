using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreEnd : MonoBehaviour
{
    private string _name;
    public string Name
    {
        get { return _name; }

        set
        {
            _name = value;
            textName.text = _name + " :";
        }
    }
    public TextMeshProUGUI textName;

    private string _score;
    public string Score
    {
        get { return _score; }

        set
        {
            _score = value;
            textScore.text = _score;
        }
    }
    public TextMeshProUGUI textScore;

    public void SetScore(float score)
    {
        Score = score.ToString();
    }
    public void SetName(string name)
    {
        Name = name;
    }
}
