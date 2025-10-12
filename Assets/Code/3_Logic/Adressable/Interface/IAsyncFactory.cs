using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public interface IAsyncFactory<T> where T : class
{
    Task<(T asset, AsyncOperationHandle handle)> CreateAsync();
}
