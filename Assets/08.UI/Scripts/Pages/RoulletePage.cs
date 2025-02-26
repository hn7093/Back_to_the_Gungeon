using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoulletePage : MonoBehaviour
{
    public Button StartButton;
    public Transform Roulette;
    
    public float startSpeed = 500f; // 초기 회전 속도
    public float deceleration = 100f; // 감속률 (값 조정)

    private Coroutine rotationCoroutine;

    private void Start()
    {
        StartButton.onClick.AddListener(() =>
        {
            StartButton.interactable = false; // 버튼 비활성화
            Roulette.rotation = Quaternion.Euler(0, 0, 0);

            // why: 다시 해당 UI 호출되는 경우
            if (rotationCoroutine != null) { StopCoroutine(rotationCoroutine); }
            rotationCoroutine = StartCoroutine(RotateRoulette());
        });
    }

    private IEnumerator RotateRoulette()
    {
        float currentSpeed = startSpeed;
        
        Debug.Log("회전");
        
        while (currentSpeed > 1f)
        {
            Roulette.Rotate(new Vector3(0, 0, -1 * (currentSpeed * Time.deltaTime)));
            currentSpeed -= deceleration * Time.deltaTime; // 감속
            
            yield return null;
        }
    }
}
