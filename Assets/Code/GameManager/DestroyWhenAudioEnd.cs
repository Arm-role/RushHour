using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenAudioEnd : MonoBehaviour
{
    private AudioSource audioSource;
    public bool isDestroy = true;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDestroy)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }
}
