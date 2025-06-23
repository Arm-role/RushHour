public interface ISwitch
{
    void Enter();
    void Exit();
}
public interface ISwitch<T> where T : class
{
    void Enter(T iswitch);
    void Exit(T iswitch);
}