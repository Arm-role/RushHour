using System;
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
        PlayerEvents.Instance.OnSentPlayerScore.Subscribe(CreateAllPlayer);

        TotalScore = 0;
    }
    private void OnDestroy()
    {
        PlayerEvents.Instance.OnSentPlayerScore.UnSubscribe(CreateAllPlayer);
    }
    public void CreateAllPlayer((string, float) player)
    {
        Debug.Log(player);

        GameObject playerObject = Instantiate(PlayerPrefab, AllPlayerParent);
        playerObject.transform.localScale = Vector3.one;

        if (playerObject.transform.TryGetComponent<PlayerScoreEnd>(out PlayerScoreEnd scoreEnd))
        {
            string score = player.Item2.ToString();
            scoreEnd.Name = player.Item1;
            scoreEnd.Score = score;
            playerObject.gameObject.name = score;

            TotalScore += player.Item2;
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
}
