using System;
using System.Collections.Generic;
using UnityEngine;
public class CutSceneHandle : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> tutorialObject = new List<GameObject>();

    private int currentIndex = 0;
    void Start()
    {
        foreach (Transform child in transform)
        {
            tutorialObject.Add(child.gameObject);
        }
    }
    public void Next()
    {
        currentIndex++;
        int nextIndex = (currentIndex + 1) % tutorialObject.Count;

        for (int i = 0; i < tutorialObject.Count; i++)
        {
            if (nextIndex == i)
            {
                ActiveObject(i);
            }
            else
            {
                UnActiveObject(i);
            }
        }
    }
    public void Previous()
    {
        currentIndex--;
        int previousIndex = (currentIndex - 1 + tutorialObject.Count) % tutorialObject.Count;

        for (int i = 0; i < tutorialObject.Count; i++)
        {
            if (previousIndex == i)
            {
                ActiveObject(i);
            }
            else
            {
                UnActiveObject(i);
            }
        }
    }
    private void ActiveObject(int index)
    {
        tutorialObject[index].SetActive(true);
    }
    private void UnActiveObject(int index)
    {
        tutorialObject[index].SetActive(false);
    }
}
