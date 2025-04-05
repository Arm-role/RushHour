using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMonoBase<SceneController>
{
    public event Action<string> OnSceneLoaded;
    public event Action<string> OnSceneUnloaded;

    public void LoadScene(string sceneName, bool usePooling = false)
    {
        if (usePooling && ScenePool.IsSceneLoaded(sceneName))
        {
            Debug.Log($"[SceneManager] Using pooled scene: {sceneName}");
            ScenePool.ActivateScene(sceneName);
            OnSceneLoaded?.Invoke(sceneName);
            return;
        }

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single)
            .completed += operation =>
            {
                Debug.Log($"[SceneManager] Scene Loaded: {sceneName}");
                OnSceneLoaded?.Invoke(sceneName);
            };
    }

    public void UnloadScene(string sceneName, bool usePooling = false)
    {
        if (usePooling)
        {
            ScenePool.DeactivateScene(sceneName);
        }
        else
        {
            SceneManager.UnloadSceneAsync(sceneName).completed += operation =>
            {
                Debug.Log($"[SceneManager] Scene Unloaded: {sceneName}");
                OnSceneUnloaded?.Invoke(sceneName);
            };
        }
    }
}
