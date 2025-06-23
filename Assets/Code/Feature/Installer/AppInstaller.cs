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

        Container.Register<IAdressablePoolService>(new AdressablePoolingService());
        Container.Register<IAdressableCacheService>(new AdressableCacheService());
        Container.Register(new PlayerManager());

        IsReady = true;
        OnServiceReady?.Invoke(Container);
    }
}
