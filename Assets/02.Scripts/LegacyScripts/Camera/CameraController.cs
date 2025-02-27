using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    StageManager stageManager;
    [SerializeField] private GameObject player;
    [SerializeField] private float smoothSpeed = 0.025f;
    [SerializeField] private Vector3 offset = new Vector3(0,0,-10);
    [SerializeField] private Vector2 minBoundary;
    [SerializeField] private Vector2 maxBoundary;

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
    }

    void LateUpdate()//�ε巯�� ī�޶� ������ ���� lerp�� lateUpdate ���
    {
        if (stageManager != null && (stageManager.stageCount == 11 || stageManager.stageCount == 21))
        {
            minBoundary = new Vector2(-2.1f, -0.75f);
            maxBoundary = new Vector2(2f, 6f);
        }
        else
        {
            minBoundary = new Vector2(-0.85f, -1.59f);
            maxBoundary = new Vector2(0.72f, 6.75f);
        }
        
        Vector3 desiredPosition = player.transform.position + offset;
        desiredPosition.x = Mathf.Clamp(player.transform.position.x, minBoundary.x, maxBoundary.x) + offset.x;
        desiredPosition.y = Mathf.Clamp(player.transform.position.y, minBoundary.y, maxBoundary.y) + offset.y;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
