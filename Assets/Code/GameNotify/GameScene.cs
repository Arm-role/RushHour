public class GameScene
{
    public void OnSceneGame(EGameScene scene)
    {
        switch (scene)
        {
            case EGameScene.Login:
                OnScenLogin();
                break;
            case EGameScene.Lobby:
                OnScenLobby();
                break;
            case EGameScene.Game:
                OnScenGame();
                break;
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
