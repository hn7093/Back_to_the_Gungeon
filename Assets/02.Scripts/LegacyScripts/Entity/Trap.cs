using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject trap;
    public GameObject trapEffect;
    public int trapDamage = 20;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Trap activated");
            // 폭발 이펙트 생성
            GameObject effect = Instantiate(trapEffect, transform.position, Quaternion.identity);
            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
            
            // 닿은 플레이어에게 데미지
            ResourceController resourceController = collision.gameObject.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                float damage = -10f;
                resourceController.ChangeHealth(damage);
                Debug.Log("Player take trap Damage!");
            }
            
            if (particleSystem != null)
            {
                Destroy(effect, particleSystem.main.duration + 0.5f); // 이펙트 재생 후 0.5초 뒤 생성된 이펙트 제거
            }
            Destroy(trap); // 사용한 트랩 제거
        }
    }
}
