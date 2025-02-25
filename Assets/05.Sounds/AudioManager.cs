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
    public AudioSource _audioSource;

    public void ChangeBGM(int number)
    {
        if (number < 0 || number >= bgmList.Count)
        {
            Debug.LogError("cannot found BGM source");
            return;
        }
        
        currentBGM = bgmList[number];
        _audioSource.clip = currentBGM.clip;
        _audioSource.Play();
    }
    
    public void ChangeBGM(string name)
    {
        if (bgmList.Select(list => list.name).Contains(name))
        {
            _audioSource.clip = bgmList.Find(list => list.name == name).clip;
            
            _audioSource.Stop();
            _audioSource.Play();
        }
        // if (bgmList.Select(list => list.name).ToList().Contains(name))
        // {
            // Debug.LogError("cannot found BGM source");
            // return;
        // }
        
        // _audioSource.clip = bgmList.Find(list => list.name == name).clip;
        // _audioSource.Play();
    }

    public void TurnBGMOn(bool isOn)
    {
        if(isOn) _audioSource.Play();
        else _audioSource.Stop();
    }
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ChangeBGM(0);
    }
}
