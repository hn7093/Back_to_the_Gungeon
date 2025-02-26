using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBackground;  // ���̽�ƽ UI ��� (Dynamic)
    public RectTransform joystickHandle;      // ���̽�ƽ �ڵ�
    private Vector2 inputVector;
    private bool isJoystickActive = false;

    public Vector2 Direction => inputVector;

    private void Start()
    {
        joystickBackground.gameObject.SetActive(false); // ������ �� ���̽�ƽ �����
    }

    private void Update()
    {
        // Ŭ���ϸ� ���̽�ƽ�� �ش� ��ġ�� ǥ��
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            Vector2 touchPosition = Input.mousePosition;
            ShowJoystick(touchPosition);
        }
    }

    public void ShowJoystick(Vector2 position)
    {
        joystickBackground.position = position;
        joystickBackground.gameObject.SetActive(true);
        joystickHandle.anchoredPosition = Vector2.zero;
        isJoystickActive = true;
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
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
        joystickBackground.gameObject.SetActive(false); // ��ġ ���� �� ���̽�ƽ �����
        isJoystickActive = false;
    }

    // Ư�� UI ��Ҹ� Ŭ���ߴ��� Ȯ��
    private bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
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
