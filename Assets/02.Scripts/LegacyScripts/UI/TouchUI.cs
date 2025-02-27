using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform joystickBackground;  // 조이스틱 배경
    public RectTransform joystickHandle;      // 조이스틱 핸들
    private Vector2 inputVector;
    private bool isJoystickActive = false;

    public Vector2 Direction => inputVector;

    private void Start()
    {
        joystickBackground.gameObject.SetActive(false); // 시작할 때 조이스틱 숨기기
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 특정 UI 요소를 클릭했는지 확인 후 무시
        if (IsPointerOverUI()) return;

        // 터치한 위치에서 조이스틱 활성화
        ShowJoystick(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isJoystickActive) return;

        Vector2 dragPosition = eventData.position - (Vector2)joystickBackground.position;
        float radius = joystickBackground.sizeDelta.x / 2;
        inputVector = Vector2.ClampMagnitude(dragPosition / radius, 1.0f);
        joystickHandle.anchoredPosition = inputVector * radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isJoystickActive) return;

        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
        joystickBackground.gameObject.SetActive(false); // 터치 종료 시 조이스틱 숨기기
        isJoystickActive = false;
    }

    private void ShowJoystick(Vector2 position)
    {
        joystickBackground.position = position;
        joystickBackground.gameObject.SetActive(true);
        joystickHandle.anchoredPosition = Vector2.zero;
        isJoystickActive = true;
    }

    // 특정 UI 요소를 클릭했는지 확인
    private bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("UIExclude")) // 특정 UI 태그를 가진 요소 제외
            {
                return true;
            }
        }
        return false;
    }
}
