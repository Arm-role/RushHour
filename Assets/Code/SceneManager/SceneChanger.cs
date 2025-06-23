using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private int count = 0;
    private string _sceneName;
    public void LoadScene(string Name)
    {
        SceneController.Instance.LoadScene(Name);
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
            SceneController.Instance.LoadScene(_sceneName);
        }
    }
    public void activeModelWindow(string modelName)
    {
        Debug.Log(modelName);
    }
    public void ActiveObject(GameObject OB)
    {
        OB.SetActive(true);
    }
    public void UnActiveObject(GameObject OB)
    {
        OB.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
