using System;
using System.Threading.Tasks;

public class TaskObserver
{
    public TaskStatus Status => Task.Status;
    public bool IsCompleted { get; protected set; } = false;
    public bool IsFaulted => Task.IsFaulted;
    public Exception Exception => Task.Exception;
    public Task Task { get; }

    private Action _onSuccess;
    private Action<Exception> _onError;
    public TaskObserver(Task task)
    {
        Task = task;
        WatchAsyncTask();
    }

    public TaskObserver OnCompleted(Action callback)
    {
        _onSuccess = callback;
        return this;
    }
    public TaskObserver OnError(Action<Exception> callback)
    {
        _onError = callback;
        return this;
    }

    private async void WatchAsyncTask()
    {
        try
        {
            await Task;
            IsCompleted = true;
            _onSuccess?.Invoke();
        }
        catch (Exception ex)
        {
            IsCompleted = true;
            _onError?.Invoke(ex);
        }
    }
}

public class TaskObserver<T> : TaskObserver
{
    public T Result { get; private set; }

    private Action<T> _onSuccess;
    private Action<Exception> _onError;
    public TaskObserver(Task<T> task) : base(task)
    {
        WatchTaskAsync(task);
    }

    public TaskObserver<T> OnCompleted(Action<T> callback)
    {
        if (IsCompleted && !IsFaulted)
        {
            callback?.Invoke(Result);
        }
        else 
        {
            _onSuccess = callback;
        }
        return this;
    }

    public new TaskObserver<T> OnError(Action<Exception> callback)
    {
        if (IsCompleted && IsFaulted)
        {
            callback?.Invoke(Exception);
        }
        else
        {
            _onError = callback;
        }
        return this;
    }

    private async void WatchTaskAsync(Task<T> task)
    {
        try
        {
            Result = await task;
            IsCompleted = true;
            _onSuccess?.Invoke(Result);
        }
        catch (Exception ex)
        {
            IsCompleted = true;
            _onError?.Invoke(ex);
        }
    }
}