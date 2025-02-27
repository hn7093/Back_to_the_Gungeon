using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    [SerializeField] private List<GameObject> heartObjects;
    [SerializeField] private Sprite fullHeart;  // 체력이 있는 하트
    [SerializeField] private Sprite halfHeart;  // 체력이 있는 하트
    [SerializeField] private Sprite emptyHeart; // 체력이 없는 하트

    private ResourceController resourceController; // 체력 관리 클래스 연결
    private int maxHearts; // 최대 하트 개수

    private void Awake()
    {
        resourceController = GetComponent<ResourceController>(); // 씬에서 찾기
    }

    private void Start()
    {
        if (resourceController != null)
        {
            maxHearts = heartObjects.Count;
            // 체력 변경 이벤트 등록
            resourceController.AddHealthChangeEvent(UpdateHealthUI);
            UpdateHealthUI(resourceController.CurrentHealth, resourceController.MaxHealth);
        }
    }

    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        float healthPerHeart = maxHealth / maxHearts;
        float remainingHealth = currentHealth;

        for(int i = 0; i < heartObjects.Count; i++)
        {
            SpriteRenderer renderer = heartObjects[i].GetComponent<SpriteRenderer>();

            if (remainingHealth >= healthPerHeart)
                renderer.sprite = fullHeart;
            else if(remainingHealth > healthPerHeart/2)
                renderer.sprite = halfHeart;
            else
                renderer.sprite = emptyHeart;

            remainingHealth -= healthPerHeart;
        }

        if (currentHealth == 0)
            OnDestroy();
    }

    private void OnDestroy()
    {
        if (resourceController != null)
        {
            resourceController.RemoveHealthChangeEvent(UpdateHealthUI);
        }
    }
}
