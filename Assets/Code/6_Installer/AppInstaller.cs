using GameEvents;
using System;
using System.Collections;
using UnityEngine;

public class AppInstaller : MonoBehaviour
{
    public static DIContainerBase Container;
    private static bool _isInitialzed = false;

    public static bool IsReady { get; private set; } = false;
    public static event Action<DIContainerBase> OnServiceReady;
    private void Awake()
    {
        if (_isInitialzed)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _isInitialzed = true;

        Container =  new DIContainerBase();

        var SceneHandle = new SceneHandle();

        var gameScene = new SceneSettup();
        var gameFlow = new GameFlowSettup();

        IAdressablePoolService poolService = new AdressablePoolingService();
        IAdressableCacheService cacheService = new AdressableCacheService();

        Container.Register(SceneHandle);

        Container.Register(gameScene);
        Container.Register(gameFlow);

        Container.Register(poolService);
        Container.Register(cacheService);

        IsReady = true;
        OnServiceReady?.Invoke(Container);
    }
    private void Start()
    {
        EventManager.Invoke(new GameFlow(EGameFlow.GameStart));
    }
}
