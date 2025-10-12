using System;
using System.Reflection;
public abstract class SingletonBase<T> where T : SingletonBase<T>
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _isCreateInstacne = false;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_isCreateInstacne)
                    {
                        throw new Exception($"Circular dependency detected while creating {typeof(T)}!");
                    }
                    _isCreateInstacne = true;

                    try
                    {
                        ConstructorInfo constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                        if (constructor == null)
                        {
                            throw new Exception($"No private/protected constructor found for {typeof(T)}.");
                        }
                        _instance = (T)constructor.Invoke(null);
                    }
                    finally
                    {
                        _isCreateInstacne = false;
                    }
                }
            }
            return _instance;
        }
    }

    protected SingletonBase()
    {
        if (_instance != null)
        {
            throw new Exception($"An instance of {typeof(T)} already exists! Use {nameof(Instance)} instead.");
        }
    }
}
