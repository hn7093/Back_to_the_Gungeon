using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 기존 체력, 속도 저장소
public class StatHandler : MonoBehaviour
{
    [Range(1, 1000)][SerializeField] private int health = 10;
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 1000);
    }

    [Range(1f, 20f)][SerializeField] private float speed = 3;
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }
}
