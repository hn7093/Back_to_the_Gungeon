using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[System.Serializable]
public class AudioData
{
    public string name;
    public AudioClip clip;
}

//todo: 프로젝타일 매니저와 함께 사운드 관리
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public List<AudioData> bgmList;
    [HideInInspector] public AudioData currentBGM;
    // 볼륨 변경 로직 필요
    private AudioSource bgmAudioSource;
    
    public List<AudioData> soundList;
    [HideInInspector] public AudioData currentSound;
    private AudioSource soundAudioSource;
    private float currentVfxSoundVolume = 0.5f;
    


    public void UpdateBGMSourceClip(int number)
    {
        if (number < 0 || number >= bgmList.Count)
        {
            Debug.LogError("cannot found BGM source");
            return;
        }
        
        currentBGM = bgmList[number];
        bgmAudioSource.clip = currentBGM.clip;
        bgmAudioSource.Play();
    }
    
    public void UpdateBGMSourceClip(string name)
    {
        if (!bgmList.Select(list => list.name).Contains(name)) { Debug.LogError("cannot found BGM source"); return; }
        bgmAudioSource.clip = bgmList.Find(list => list.name == name).clip;
    }

    public void TurnBGMOn(bool isOn, string soundType)
    {
        AudioSource targetAudioSource = soundType switch
        {
            "BGM" => bgmAudioSource,
            "VFX" => soundAudioSource,
            _ => throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null)
        };

        if (isOn) {
            if(!targetAudioSource.isPlaying) targetAudioSource.Play();
            targetAudioSource.mute = false;
        }
        else {
            targetAudioSource.Stop();
            targetAudioSource.mute = true;
        }
        
        targetAudioSource.time = 0;
    }
    
    // fix: 변수가 특별이 필요 없으나 통일성있게 관리하기
    public void UpdateBGMVolume(float value, string soundType)
    {
        AudioSource targetAudioSource = soundType switch
        {
            "BGM" => bgmAudioSource,
            "VFX" => soundAudioSource,
            _ => throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null)
        };
        
        if (bgmAudioSource)
        {
            targetAudioSource.volume = Mathf.Clamp01(value / 100f);
        }
    }

    public void PlayVFXSoundByName(string name)
    {
        AudioClip selectedSoundClip = soundList.Find(sound => sound.name == name).clip;
        // Debug.Log(selectedSoundClip.name);
        soundAudioSource.PlayOneShot(selectedSoundClip, 0.5f);
    }
    
    private void Awake()
    {
        // do: 예외 처리 필요
        AudioSource[] audioSources = GetComponents<AudioSource>();
        bgmAudioSource = audioSources[0];
        soundAudioSource = audioSources[1];
    }

    private void Start()
    {
        UpdateBGMSourceClip(0);
    }
}
