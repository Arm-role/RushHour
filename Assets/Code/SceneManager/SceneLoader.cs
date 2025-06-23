using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : SingletonMonoBase<SceneLoader>
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressBar;

    public void LoadSceneWithLoadingScreen(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            progressBar.value = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }

        loadingScreen.SetActive(false);
        //SceneController.Instance.OnSceneLoaded?.Invoke(sceneName);
    }
}