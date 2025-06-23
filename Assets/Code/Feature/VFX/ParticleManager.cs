using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager
{
    private ParticalService _service;
    public ParticleManager(ParticalService service)
    {
        _service = service;
    }
    public void Play(string name, Vector3 pos)
    {
        _service.Play(name, pos);
    }
}
