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
    public AudioData currentBGM;
    // 볼륨 변경 로직 필요
    private AudioSource bgmAudioSource;
    
    public List<AudioData> soundList;
    public AudioData currentSound;
    private AudioSource soundAudioSource;
    public float currentVfxSoundVolume;
    


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

    public void TurnBGMOn(bool isOn)
    {
        if (isOn) {
            if(!bgmAudioSource.isPlaying) bgmAudioSource.Play();
            bgmAudioSource.mute = false;
        }
        else {
            bgmAudioSource.Stop();
            bgmAudioSource.mute = true;
        }
    }
    
    // fix: 변수가 특별이 필요 없으나 통일성있게 관리하기
    public void UpdateBGMVolume(float value)
    {
        if (bgmAudioSource)
        {
            bgmAudioSource.volume = Mathf.Clamp01(value / 100f);
        }
    }

    public void PlayVFXSoundByName(string name)
    {
        AudioClip selectedSoundClip = soundList.Find(sound => sound.name == name).clip;
        soundAudioSource.PlayOneShot(selectedSoundClip, currentVfxSoundVolume);
    }
    
    private void Awake()
    {
        bgmAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UpdateBGMSourceClip(0);
    }
}
