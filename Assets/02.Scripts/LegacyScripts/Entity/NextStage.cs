using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class NextStage : MonoBehaviour
{
    public GameObject closeDoor;
    public GameObject openDoor;
    
    public GameObject player;
    StageManager stageManager;
    public Fade fade;

    void Awake()
    {
        fade = FindObjectOfType<Fade>();
        if(fade == null)
            Debug.LogError("Fade is null");
        player = GameObject.FindGameObjectWithTag("Player");
        stageManager = FindObjectOfType<StageManager>();
    }

    void Start()
    {
        DoorClose();
    }


    public void DoorOpen()
    {
        closeDoor.gameObject.SetActive(false);
        openDoor.gameObject.SetActive(true);
    }

    public void DoorClose()
    {
        closeDoor.gameObject.SetActive(true);
        openDoor.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("텔레포트");
            // 페이드 아웃 후
            StartCoroutine(fade.FadeOut());
            Destroy(stageManager.currentStage); // 현재 맵 파괴
            // 페이드 인 후
            StartCoroutine(fade.FadeIn());
            stageManager.GenerateNewStage();
            // 플레이어 좌표값도 변경해야함
            if (player != null)
            {
                player.transform.position = new Vector3(0, -5f, 0); // 새로운 방 시작 위치
            }

        }
    }
    
}
