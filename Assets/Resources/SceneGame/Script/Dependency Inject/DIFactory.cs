using System;

public static class DIFactory
{
    public static void Create<T>(DIContainerBase container) where T : class, new()
    {
        if (!container.TryGet<T>(out _))
        {
            var instance = new T();
            
            container.Register<T>(instance);
        }
    }
    public static T Get<T>(DIContainerBase container)
    {
        if (container.TryGet<T>(out var instance))
        {
            return instance;
        }
        throw new Exception($"[DISetup] {typeof(T).Name} is not registered in {container}!");
    }
}