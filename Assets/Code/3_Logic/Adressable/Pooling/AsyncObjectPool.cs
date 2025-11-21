using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncObjectPool<T> where T : class
{
    private readonly Queue<T> _available = new();
    private readonly IAsyncGameObjectFactory<T> _factory;
    public AsyncObjectPool(IAsyncGameObjectFactory<T> factory)
    {
        _factory = factory;
    }
    public async Task<T> GetAsync()
    {
        while (_available.Count > 0)
        {
            var obj = _available.Dequeue();

            if (obj is MonoBehaviour mb)
            {
                if (mb.gameObject.activeSelf)
                    continue;
            }

            return obj;
        }

        return await _factory.CreateAsync();
    }
    public async void Return(T obj)
    {
        if (obj is MonoBehaviour mb)
        {
            await Task.Yield();
            if (mb == null) return;
        }
        _available.Enqueue(obj);
    }
}
