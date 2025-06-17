using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour, IPoolable
{
    private System.Action<GameObject> returnToPool;
    private AudioSource audioSource;

    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPitchVariance)
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

            CancelInvoke();
            audioSource.clip = clip;
            audioSource.volume = soundEffectVolume;
            audioSource.PlayOneShot(clip);
            audioSource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);

            Invoke("Disable", clip.length + 0.1f);
    }

    public void Disable()
    {
        audioSource?.Stop();
        OnDespawn();
    }

    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {
       returnToPool?.Invoke(gameObject);
    }

    public void Initialize(System.Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
    }
}
