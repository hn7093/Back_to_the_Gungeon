using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject trap;
    public GameObject trapEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Trap activated");
            GameObject effect = Instantiate(trapEffect, transform.position, Quaternion.identity);
            ParticleSystem particleSystem = trapEffect.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                Destroy(effect, particleSystem.main.duration + 0.5f);
            }
            Destroy(trap); // 사용한 트랩 제거
        }
    }
}
