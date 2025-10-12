using GameEvents;
using UnityEngine;

public class ParticleManager
{
    private ParticalService _service;
    public ParticleManager(ParticalService service)
    {
        _service = service;

        EventManager.Subscribe<PlayParticle>(Play);
    }
    private void Play(PlayParticle evt)
    {
        _service.Play(evt.ParticleName, evt.Position);
    }
}
