using System;
using UnityEngine;

public class ModeSceneInstaller : SceneInstaller
{
    protected override void Start() => base.Start();
    protected override void OnDestroy() => base.OnDestroy();

    protected override void Initialzed(DIContainerBase globalContainer)
    {
        Destroy(gameObject);
    }
}
