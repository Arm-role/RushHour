using UnityEngine.SceneManagement;

public class DIManager : SingletonMonoBase<DIManager>
{
    public DIContainerBase GlobalContainer { get; private set; } = new DIContainerBase();
    public DIContainerBase SceneGameContainer { get; private set; } = new DIContainerBase();

    protected override void Awake()
    {
        base.Awake();

        SingletonLazy<GameEvents>.SetContainer(GlobalContainer);
        SingletonLazy<ItemEvents>.SetContainer(GlobalContainer);
        SingletonLazy<PlayerEvents>.SetContainer(GlobalContainer);
        SingletonLazy<OtherEvents>.SetContainer(GlobalContainer);

        DIFactory.Create<GameEvents>(GlobalContainer);
        DIFactory.Create<ItemEvents>(GlobalContainer);
        DIFactory.Create<PlayerEvents>(GlobalContainer);
        DIFactory.Create<OtherEvents>(GlobalContainer);

        GameEvents gameEvents = GlobalContainer.GetScript<GameEvents>();
        gameEvents.OnGameScene.Invoke(EGameScene.Login);

        OnSceneGame();
    }
    public void OnSceneGame()
    {
        SingletonLazy<SpawnManager>.SetContainer(SceneGameContainer);

        DIFactory.Create<SpawnManager>(SceneGameContainer);
    }
}