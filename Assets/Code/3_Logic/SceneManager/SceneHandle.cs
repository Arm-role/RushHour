using SceneEvents;

public class SceneHandle
{
    private SceneController _sceneController = new SceneController();

    public SceneHandle()
    {
        EventManager.Subscribe<LoadScene>(LoadScene);
        EventManager.Subscribe<UnloadScene>(UnloadScene);
    }

    private void LoadScene(LoadScene loadScene)
    {
        _sceneController.LoadScene(loadScene.SceneName, loadScene.UsePooling);
    }

    private void UnloadScene(UnloadScene unLoadScene)
    {
        _sceneController.UnloadScene(unLoadScene.SceneName, unLoadScene.UsePooling);
    }
}

namespace SceneEvents
{
    public readonly struct LoadScene
    {
        public readonly string SceneName;
        public readonly bool UsePooling;

        public LoadScene(string sceneName, bool usePooling = false)
        {
            SceneName = sceneName;
            UsePooling = usePooling;
        }
    }
    public readonly struct UnloadScene
    {
        public readonly string SceneName;
        public readonly bool UsePooling;

        public UnloadScene(string sceneName, bool usePooling = false)
        {
            SceneName = sceneName;
            UsePooling = usePooling;
        }
    }
}