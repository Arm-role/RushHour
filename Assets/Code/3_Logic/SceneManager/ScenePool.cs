using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScenePool
{
    private static Dictionary<string, Scene> _scenePool = new Dictionary<string, Scene>();

    public static bool IsSceneLoaded(string sceneName)
    {
        return _scenePool.ContainsKey(sceneName);
    }

    public static void LoadSceneToPool(string sceneName)
    {
        if (!_scenePool.ContainsKey(sceneName))
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += operation =>
            {
                Scene scene = SceneManager.GetSceneByName(sceneName);
                _scenePool[sceneName] = scene;
                Debug.Log($"[ScenePool] Scene {sceneName} added to pool.");
            };
        }
    }

    public static void UnloadSceneFromPool(string sceneName)
    {
        if (_scenePool.ContainsKey(sceneName))
        {
            SceneManager.UnloadSceneAsync(sceneName);
            _scenePool.Remove(sceneName);
            Debug.Log($"[ScenePool] Scene {sceneName} removed from pool.");
        }
    }

    public static void ActivateScene(string sceneName)
    {
        if (_scenePool.ContainsKey(sceneName))
        {
            SceneManager.SetActiveScene(_scenePool[sceneName]);
            Debug.Log($"[ScenePool] Scene {sceneName} activated.");
        }
    }

    public static void DeactivateScene(string sceneName)
    {
        if (_scenePool.ContainsKey(sceneName))
        {
            SceneManager.UnloadSceneAsync(sceneName);
            Debug.Log($"[ScenePool] Scene {sceneName} deactivated (still in pool).");
        }
    }
}
