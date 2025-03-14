using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
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
        PlayerManager.Instance.OnSentPlayerNetwork += CreateAllPlayer;
        EvenManager.SentTotalScore += SetTotalScore;

        TotalScore = 0;
    }
    public void CreateAllPlayer(PlayerNetwork player)
    {
        Debug.Log("CreatChild");
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
