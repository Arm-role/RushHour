using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private int count = 0;
    private string nameScene;
    public void changeTo(string index)
    {
        SceneManager.LoadScene(index);
    }
    public void changeToAndCount(string index)
    {
        nameScene = index;
    }
    public void changeCount(int limit)
    {
        count++;
        if (count == limit)
        {
            count = 0;
            SceneManager.LoadScene(nameScene);
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
