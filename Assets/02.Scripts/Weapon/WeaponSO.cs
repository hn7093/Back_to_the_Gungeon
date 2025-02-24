using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType
{
    Ranged,
    Melee
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponSO/WeaponData")]
public class WeaponSO : ScriptableObject
{
    [Header("Attack Info")]
    public WeaponType weaponType = WeaponType.Ranged;
    public float delay = 1f; // 발사 간격
    public float weaponSize = 1f; // 이미지 크기
    public float power = 1f; // 공격력
    public float speed = 1f; // 발사체 속도
    public float attackRange = 10f; // 탐색 범위
    public AudioClip attackSoundClip;


    [Header("Knock Back Info")]
    public bool isOnKnockback = false; // 넉백여부
    public float knockbackPower = 0.1f; // 넉백 정도
    public float knockbackTime = 0.5f; // 넉백 시간
    

    [Header("Melee Attack Info")]
    public Vector2 collideBoxSize = Vector2.zero; // 충돌체 크기

    [Header("Ranged Attack Data")]
    public Transform projectileSpawnPosition; // 발사 위치
    public int bulletIndex; // 발사체 인덱스
    public float bulletSize = 1.0f; // 발사체 크기
    public float duration;
    public float spread;
    public int numberofProjectilesPerShot = 1; // 발사체 갯수
    public float multipleProjectilesAngel = 10.0f; // 퍼짐 각도
    public Color projectileColor = Color.white; // 발사체 색

}
