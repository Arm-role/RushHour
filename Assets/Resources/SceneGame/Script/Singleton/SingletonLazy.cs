using System;

public abstract class SingletonLazy<T>
{
    private static DIContainerBase _containerBase;

    public static void SetContainer(DIContainerBase containerBase)
    {
        _containerBase = containerBase;
    }
    public static T Instance
    {
        get
        {
            if (_containerBase == null)
            {
                throw new Exception($"Container for {typeof(T).Name} is not set!");
            }
            return DIFactory.Get<T>( _containerBase );
        }
    }
    protected SingletonLazy() => OnAwake();
    protected virtual void OnAwake() { }
}