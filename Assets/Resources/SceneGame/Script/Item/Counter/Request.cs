using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Request : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public TextMeshProUGUI one;
    public TextMeshProUGUI two;
    public GameObject display;
    public GameObject sever;

    private void Start()
    {
        EvenManager.foodStateDisplay += SetDisplay;
        EvenManager.NextIngredient += SetState;
    }
    public void SetState(Sprite sprite, int amount, int limmit)
    {
        SetDisplay(false);
        Renderer.sprite = sprite;
        one.text = amount.ToString();
        two.text = limmit.ToString();

    }
    public void SetDisplay(bool displayed)
    {
        display.SetActive(!displayed);
        sever.SetActive(displayed);
    }
}
