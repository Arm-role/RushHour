using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    public float ScaleParticle;
    public GameObject[] Particle;
    public Dictionary<string, GameObject> particles = new Dictionary<string, GameObject>();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (GameObject ob in Particle)
        {
            if (!particles.ContainsKey(ob.name))
            {
                particles.Add(ob.name, ob);
            }
        }
    }


    public void CreateParticle(string name, Vector3 pos)
    {
        if (particles.ContainsKey(name))
        {
            GameObject ob = Instantiate(particles[name]);
            ob.transform.localScale = Vector3.one * ScaleParticle;
            ob.transform.position = pos;
        }
    }
}
