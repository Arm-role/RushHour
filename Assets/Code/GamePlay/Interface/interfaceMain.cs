using System;
using System.Collections;

public interface ISceneState : ISwitch { }

public interface IEvent
{
    void Subscribe(Action action);
    void UnSubscribe(Action action);
    void Invoke();
}

public interface ISceneLoader
{
    IEnumerator LoadScene(string sceneName);
}