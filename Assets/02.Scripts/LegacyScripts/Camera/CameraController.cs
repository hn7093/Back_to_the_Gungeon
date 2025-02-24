using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float smoothSpeed = 0.025f;
    [SerializeField] private Vector3 offset = new Vector3(0,0,-10);

    void LateUpdate()//�ε巯�� ī�޶� ������ ���� lerp�� lateUpdate ���
    {
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
