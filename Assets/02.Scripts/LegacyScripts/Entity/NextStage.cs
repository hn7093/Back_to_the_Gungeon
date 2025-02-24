using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NextStage : MonoBehaviour
{
    public GameObject closeDoor;
    public GameObject openDoor;
    
    StageManager stageManager;


    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        openDoor = collision.gameObject;
        
        // 페이드 아웃 후
        Destroy(stageManager.currentStage); // 현재 맵 파괴
        // 페이드 인 후
        stageManager.GenerateNewStage();
        // 플레이어 좌표값도 변경해야함
    }

    
}
