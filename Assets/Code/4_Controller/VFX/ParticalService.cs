using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class ParticalService
{
    private readonly IAdressablePoolService _poolService;
    private readonly ParticleLibrary _particleLibrary;

    public ParticalService(IAdressablePoolService poolService, ParticleLibrary particleLibrary)
    {
        _poolService = poolService;
        _particleLibrary = particleLibrary;
    }

    public async void Play(string particleName, Vector3 position)
    {
        var assetRef = _particleLibrary.Find(particleName);
        if(assetRef == null || !assetRef.RuntimeKeyIsValid()) return;

        GameObject instance = await _poolService.AsyncGet(assetRef);

        instance.transform.position = position;
        instance.SetActive(true);
        var pSystem = instance.GetComponent<ParticleSystem>();
        pSystem.Play();

        ReturnAfterDelay(assetRef, instance, pSystem.main.duration);
    }

    private async void ReturnAfterDelay(AssetReferenceGameObject assetReference, GameObject instance, float delay)
    {
        await Task.Delay((int)(delay * 1000));
        _poolService.Return(assetReference, instance);
    }
}
