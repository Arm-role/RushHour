using UnityEngine;

public class SoundService : MonoBehaviour
{
    [SerializeField] private SoundLibrary _library;
    private AudioSource _audioSource;

    public void PlayMusic(string clipName)
    {
        var se = _library.Get(clipName);
        if (se.Clip == null)
        {
            Debug.LogWarning($"[SoundService] Clip '{clipName}' not found");
            return;
        }
        Debug.Log($"[SoundService] Clip '{clipName}' found");

        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }

        _audioSource.clip = se.Clip;
        _audioSource.spatialBlend = 0f;
        _audioSource.volume = se.Volume;
        _audioSource.loop = se.Loop;
        _audioSource.Play();
    }
    public void StopMusic()
    {
        _audioSource.Stop();
    }
    public void PlaySFX(string clipName, Vector2 position, float? time = null)
    {
        var clip = _library.Get(clipName);
        if (clip.Clip == null)
        {
            Debug.LogWarning($"[SoundService] Clip '{clipName}' not found");
            return;
        }

        PlayClipAtPoint2D(clip, position, time);
    }

    private void PlayClipAtPoint2D(SoundEntry se, Vector2 position, float? time)
    {
        GameObject temp = new GameObject("SFX_" + se.Id);
        temp.transform.position = position;

        AudioSource source = temp.AddComponent<AudioSource>();
        source.clip = se.Clip;
        source.spatialBlend = 0f;
        source.volume = se.Volume;
        source.loop = se.Loop;
        source.Play();

        if(time != null)
        {
            Destroy(temp, time.Value);
        }
        else
        {
            Destroy(temp, se.Clip.length);
        }
    }
}