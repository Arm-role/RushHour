using Fusion;
using UnityEngine;

public abstract class StateMachine<T> : MonoBehaviour where T : StateMachine<T>
{
    protected IState<T> currentState;
    private T Owner;

    protected virtual void Awake()
    {
        Owner = (T)this;
    }

    public void SetState(IState<T> newState)
    {
        currentState?.Exit(Owner);
        currentState = newState;
        currentState?.Enter(Owner);
    }

    public void Execute()
    {
        currentState?.Execute(Owner);
    }
}
public abstract class Network_StateMachine<T> : NetworkBehaviour where T : Network_StateMachine<T>
{
    protected IState<T> currentState;
    private T Owner;

    protected virtual void Awake()
    {
        Owner = (T)this;
    }

    public void SetState(IState<T> newState)
    {
        currentState?.Exit(Owner);
        currentState = newState;
        currentState?.Enter(Owner);
    }

    public void Execute()
    {
        currentState?.Execute(Owner);
    }
}