using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI totalScore;

    public void Setup(float totalScore)
    {
        this.totalScore.text = $"{totalScore}P";
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    public void Show()
    {
        panel.SetActive(true);
    }
}