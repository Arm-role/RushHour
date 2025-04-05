using UnityEngine;
public abstract class SingletonMonoBase<T> : MonoBehaviour where T : SingletonMonoBase<T>
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _isQuitting = false;

    public static T Instance
    {
        get
        {
            if (_isQuitting)
            {
                Debug.LogWarning($"[SingletonMachine] Instance of {typeof(T)} is already destroyed. Returning null.");
                return null;
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    Debug.LogError($"[SingletonMachine] No instance of {typeof(T)} found! Make sure it exists in the scene.");
                    throw new System.Exception($"Singleton instance of {typeof(T)} does not exist!");
                }
                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = (T)this;
                if (transform.root == transform)
                {
                    DontDestroyOnLoad(transform);
                }
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"[SingletonMachine] Duplicate instance of {typeof(T)} detected. Destroying {gameObject.name}.");
                Destroy(gameObject);
            }
        }

    }

    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }
    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            _isQuitting = false;
        }
    }
}
