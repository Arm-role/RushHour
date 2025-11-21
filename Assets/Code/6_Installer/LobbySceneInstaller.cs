using System;
using UnityEngine;

public class LobbySceneInstaller : SceneInstaller
{
    protected override void Start() => base.Start();
    protected override void OnDestroy() => base.OnDestroy();

    protected override void Initialzed(DIContainerBase globalContainer)
    {
        AppInstaller.OnServiceReady -= Initialzed;

        var container = new DIContainerBase(globalContainer);

        GameState gameState = FindObjectOfType<GameState>();

        AppInstaller.Container.Register(gameState);

        Destroy(gameObject);
    }
}