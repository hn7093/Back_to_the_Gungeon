using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float InvincibleTime = 0.5f;

    private BaseController baseController;
    private StatHandler statHandler;
    private AnimationHandler animationHandler;
    
    private float timeSinceLastChange = float.MaxValue;

    [SerializeField] public float CurrentHealth;
    public float MaxHealth => statHandler.Health;
    public AudioClip damageClip;
    private Action<float, float> OnChangeHealth;
    private void Awake()
    {
        statHandler = GetComponent<StatHandler>();
        animationHandler = GetComponent<AnimationHandler>();
        baseController = GetComponent<BaseController>();
    }

    private void Start()
    {
        CurrentHealth = statHandler.Health;
    }

    private void Update()
    {
        if (timeSinceLastChange < InvincibleTime)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= InvincibleTime)
            {
                animationHandler.EndInvincibility();
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if (change == 0 || timeSinceLastChange < InvincibleTime)
        {
            return false;
        }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        // 0 ~ 최대 체력 보장
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        OnChangeHealth?.Invoke(CurrentHealth, MaxHealth);

        // 음수 = 데미지
        if (change < 0)
        {
            animationHandler.Damage();
            if(damageClip != null)
            {
                SoundManager.PlayClip(damageClip);
            }
        }

        if (CurrentHealth <= 0f)
        {
            Death();
        }

        return true;
    }

    private void Death()
    {
        baseController.Death();
    }

    public void AddHealthChangeEvent(Action<float, float> action)
    {
        OnChangeHealth += action;
    }
    public void RemoveHealthChangeEvent(Action<float, float> action)
    {
        OnChangeHealth -= action;
    }
}
