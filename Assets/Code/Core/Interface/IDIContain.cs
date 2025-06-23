public interface IDIContain
{
    void Register<T>(T script);
    void Unregister<T>();
    bool TryGet<T>(out T result);
    T GetObject<T>() where T : class;
}
