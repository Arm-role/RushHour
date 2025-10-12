using SceneEvents;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    private int count = 0;
    private string _sceneName;
 
    public void LoadScene(string Name)
    {
        EventManager.Invoke(new LoadScene(Name));
    }
    public void LoadSceneAndCount(string index)
    {
        _sceneName = index;
    }
    public void LoadSceneRoundCount(int limit)
    {
        count++;
        if (count == limit)
        {
            count = 0;
            EventManager.Invoke(new LoadScene(_sceneName));
        }
    }
    public void activeModelWindow(string modelName)
    {
        Debug.Log(modelName);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
