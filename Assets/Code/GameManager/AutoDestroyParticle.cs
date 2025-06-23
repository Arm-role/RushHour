using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    private ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (particle != null && !particle.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
