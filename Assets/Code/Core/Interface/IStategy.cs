public interface IStategy<T> where T : class
{
    void Execute(T istrategy);
}
