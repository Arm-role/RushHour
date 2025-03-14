using UnityEngine;

public abstract class StateMachine<T> : MonoBehaviour where T : class
{
    protected IState<T> currentState;
    private T Owner;

    protected virtual void Awake()
    {
        Owner = this as T;
    }

    public void SetState(IState<T> newState)
    {
        currentState?.Exit(Owner);
        currentState = newState;
        currentState.Enter(Owner);
    }

    public void Execute()
    {
        currentState?.Execute(Owner);
    }
}
public interface IState<T> where T : class
{
    void Enter(T istate);
    void Execute(T istate);
    void Exit(T istate);
}