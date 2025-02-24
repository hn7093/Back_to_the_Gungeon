using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    private AudioSource _adudioSource;
    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPitchVariance)
    {
        if(_adudioSource == null)
            _adudioSource = GetComponent<AudioSource>();

        CancelInvoke();
        _adudioSource.clip = clip;
        _adudioSource.volume = soundEffectVolume;
        _adudioSource.Play();
        _adudioSource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);

        Invoke("Disable", clip.length + 2);
    }

    public void Disable()
    {
        _adudioSource.Stop();
        Destroy(this.gameObject);
    }
}
