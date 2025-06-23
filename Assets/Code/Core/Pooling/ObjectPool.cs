using System.Collections.Generic;
public class ObjectPool<T> where T : class
{
    private readonly Queue<T> pool = new();
    private readonly IFactory<T> factory;
    public ObjectPool(IFactory<T> factory, int initialSize)
    {
        this.factory = factory;

        for (int i = 0; i < initialSize; i++)
        {
            pool.Enqueue(factory.Create());
        }
    }
    public T Get()
    {
        var obj =  pool.Count > 0 ? pool.Dequeue() : factory.Create();

        return obj;
    }

    public void Return(T item)
    {
        pool.Enqueue(item);
    }
}
