using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform joystickBackground;  // ���̽�ƽ ���
    public RectTransform joystickHandle;      // ���̽�ƽ �ڵ�
    private Vector2 inputVector;
    private bool isJoystickActive = false;

    public Vector2 Direction => inputVector;

    private void Start()
    {
        joystickBackground.gameObject.SetActive(false); // ������ �� ���̽�ƽ �����
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Ư�� UI ��Ҹ� Ŭ���ߴ��� Ȯ�� �� ����
        if (IsPointerOverUI()) return;

        // ��ġ�� ��ġ���� ���̽�ƽ Ȱ��ȭ
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
        joystickBackground.gameObject.SetActive(false); // ��ġ ���� �� ���̽�ƽ �����
        isJoystickActive = false;
    }

    private void ShowJoystick(Vector2 position)
    {
        joystickBackground.position = position;
        joystickBackground.gameObject.SetActive(true);
        joystickHandle.anchoredPosition = Vector2.zero;
        isJoystickActive = true;
    }

    // Ư�� UI ��Ҹ� Ŭ���ߴ��� Ȯ��
    private bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("UIExclude")) // Ư�� UI �±׸� ���� ��� ����
            {
                return true;
            }
        }
        return false;
    }
}
