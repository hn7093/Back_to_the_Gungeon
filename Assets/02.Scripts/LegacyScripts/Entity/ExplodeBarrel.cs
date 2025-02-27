using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBarrel : MonoBehaviour
{
    [SerializeField] private GameObject player; // 플레이어 인식
    [SerializeField] private float explodingPower; // 밀어내는 힘
    [SerializeField] private float explodingDamage = -20f;
    
    public GameObject effect; // 이펙트
    public float explosionRadius = 3f;
    public LayerMask damageLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(effect, transform.position, Quaternion.identity);
        ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();
        
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayer);

        foreach (Collider2D obj in objects)
        {
            
            BaseController baseController = obj.GetComponent<BaseController>();
            //Rigidbody2D rb = obj.GetComponent<Rigidbody2D>(); 
            if (baseController != null)
            {
                float knockbackPower = explodingPower;
                float knockbackDuration = 1f;
                //Vector2 forceDirection = (obj.transform.position - transform.position).normalized; // 폭발 중심에서 객체로 향하는 벡터 계산
                //rb.AddForce(forceDirection * explodingPower, ForceMode2D.Impulse); // 오브젝트 반대 방향으로 힘 적용
                baseController.ApplyKnockback(transform, knockbackPower, knockbackDuration);
                Debug.Log($"폭발 범위 내 감지된 오브젝트: {obj.gameObject.name}");
                //Debug.Log($"➡ 적용된 힘: {forceDirection * explodingPower}");
            }
            
            ResourceController resourceController = obj.gameObject.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                resourceController.ChangeHealth(explodingDamage);
                Debug.Log("Player take explode Damage!");
            }
        }
        
        if (particleSystem != null)
        {
            Destroy(explosion, particleSystem.main.duration + 0.5f); // 이펙트 재생 후 0.5초 뒤 생성된 이펙트 제거
        }

        Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
