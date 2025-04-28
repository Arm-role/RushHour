using System;
using System.Collections;
using System.Collections.Generic;
public interface IState<T> where T : class
{
    void Enter(T istate);
    void Execute(T istate);
    void Exit(T istate);
}

public interface IDrag : IState<DragManager> { }
public interface IGameLevel : IState<GameManager> { }
public interface ICountdown : IState<Countdown> { }
public interface IPlayerNetworkState : IState<PlayerNetwork> { }



public interface IStategy<T> where T : class
{
    void Execute(T istrategy);
}

public interface IDropItemTo : IStategy<DragManager> { }
public interface IKitchenTool : IStategy<KitchenTool> { }



public interface ISwitch
{
    void Enter();
    void Exit();
}

public interface ISceneState : ISwitch { }



public interface ISwitch<T> where T : class
{
    void Enter(T iswitch);
    void Exit(T iswitch);
}
public interface IEvent<T>
{
    void Subscribe(Action<T> action);
    void Invoke(T param);
}
public interface IEvent
{
    void Subscribe(Action action);
    void UnSubscribe(Action action);
    void Invoke();
}
public interface IDiContain
{
    void Register<T>(T script);
    void Unregister<T>();
    bool TryGet<T>(out T result);
    T GetScript<T>() where T : class;
}


public interface ISceneLoader
{
    IEnumerator LoadScene(string sceneName);
}