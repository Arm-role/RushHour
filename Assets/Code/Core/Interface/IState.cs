public interface IState<T> where T : class
{
    void Enter(T istate);
    void Execute(T istate);
    void Exit(T istate);
}
