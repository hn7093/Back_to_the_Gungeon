using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    [SerializeField] private List<GameObject> heartObjects;
    [SerializeField] private Sprite fullHeart;  // ü���� �ִ� ��Ʈ
    [SerializeField] private Sprite halfHeart;  // ü���� �ִ� ��Ʈ
    [SerializeField] private Sprite emptyHeart; // ü���� ���� ��Ʈ

    private ResourceController resourceController; // ü�� ���� Ŭ���� ����
    private int maxHearts; // �ִ� ��Ʈ ����

    private void Awake()
    {
        resourceController = GetComponent<ResourceController>(); // ������ ã��
    }

    private void Start()
    {
        if (resourceController != null)
        {
            maxHearts = heartObjects.Count;
            // ü�� ���� �̺�Ʈ ���
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
