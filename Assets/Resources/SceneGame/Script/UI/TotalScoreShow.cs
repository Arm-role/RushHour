using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TotalScoreShow : MonoBehaviour
{
    [SerializeField]
    private Transform AllPlayerParent;
    public GameObject PlayerPrefab;


    private float _totalScore;
    public float TotalScore
    {
        get { return _totalScore; }
        set
        {
            _totalScore = value;
            TextTotal.text = _totalScore.ToString();
        }
    }
    public TextMeshProUGUI TextTotal;
    private void Start()
    {
        PlayerEvents.Instance.OnSentPlayerNetwork.Subscribe(CreateAllPlayer);
        GameEvents.Instance.OnSentTotalScore.Subscribe(SetTotalScore);

        TotalScore = 0;
    }
    private void OnDestroy()
    {
        PlayerEvents.Instance.OnSentPlayerNetwork.UnSubscribe(CreateAllPlayer);
        GameEvents.Instance.OnSentTotalScore.UnSubscribe(SetTotalScore);
    }
    public void CreateAllPlayer(PlayerNetwork player)
    {
        GameObject playerObject = Instantiate(PlayerPrefab, AllPlayerParent);
        playerObject.transform.localScale = Vector3.one;

        if (playerObject.transform.TryGetComponent<PlayerScoreEnd>(out PlayerScoreEnd scoreEnd))
        {
            string score = player.GetCurrentScore().ToString();
            scoreEnd.Name = player.playerName;
            scoreEnd.Score = score;
            playerObject.gameObject.name = score;

        }
        SortChild();
    }
    private void SortChild()
    {
        var children = new List<Transform>();

        foreach (Transform child in AllPlayerParent)
        {
            children.Add(child);
        }

        children = children.OrderByDescending(child =>
        {
            if (int.TryParse(child.name, out int number))
                return number;
            return int.MinValue; 
        }).ToList();

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }
    public void SetTotalScore(float score)
    {
        TotalScore += score;
    }
}
