using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType
{
    Ranged,
    Melee,
    Spin
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponSO/WeaponData")]
public class WeaponSO : ScriptableObject
{
    [Header("Attack Info")]
    public WeaponType weaponType = WeaponType.Ranged;
    public float delay = 1f; // 발사 간격
    public float weaponSize = 1f; // 이미지 크기
    public float power = 1f; // 공격력
    public float speed = 1f; // 속도
    public float attackRange = 10f; // 탐색 범위
    public AudioClip attackSoundClip;


    [Header("Knock Back Info")]
    public bool isOnKnockback = false; // 넉백여부
    public float knockbackPower = 0.1f; // 넉백 정도
    public float knockbackTime = 0.5f; // 넉백 시간
    

    [Header("Melee Attack Info")]
    public Vector2 collideBoxSize = Vector2.zero; // 충돌체 크기

    [Header("Ranged Attack Data")]
    public int bulletIndex; // 발사체 인덱스
    public float bulletSize = 1.0f; // 발사체 크기
    public float duration = 4.0f; // 발사체 지속시간
    public float spread = 10f; // 전체 탄 퍼짐 각도
    public int numberofProjectilesPerShot = 1; // 발사체 갯수
    public float multipleProjectilesAngel = 10.0f; // 발당 퍼짐 각도
    public float multipleDelay = 0.0f; // 여러 발사시 발당 딜레이
    public Color projectileColor = Color.white; // 발사체 색

    [Header("Ranged Attack Data")]
    public int numberOfObejects = 1;

}
