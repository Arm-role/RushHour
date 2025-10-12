using GameEvents;

public class SceneSettup
{
    public SceneSettup()
    {
        EventManager.Subscribe<GameScene>(OnSceneGame);
    }

    public void OnSceneGame(GameScene evt)
    {
        switch (evt.Scene)
        {
            case EGameScene.Login: OnScenLogin(); break;
            case EGameScene.Lobby: OnScenLobby(); break;
            case EGameScene.Game: OnScenGame(); break;
        }
    }

    public void OnScenLogin()
    {

    }
    public void OnScenLobby()
    {

    }
    public void OnScenGame()
    {

    }
}
