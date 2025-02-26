using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBackground;  // 조이스틱 UI 배경 (Dynamic)
    public RectTransform joystickHandle;      // 조이스틱 핸들
    private Vector2 inputVector;
    private bool isJoystickActive = false;

    public Vector2 Direction => inputVector;

    private void Start()
    {
        joystickBackground.gameObject.SetActive(false); // 시작할 때 조이스틱 숨기기
    }

    private void Update()
    {
        // 클릭하면 조이스틱을 해당 위치에 표시
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
        joystickBackground.gameObject.SetActive(false); // 터치 종료 시 조이스틱 숨기기
        isJoystickActive = false;
    }

    // 특정 UI 요소를 클릭했는지 확인
    private bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
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
