using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    public void UpdateScore(float score)
    {
        scoreText.text = $"Score: {score}";
    }
}