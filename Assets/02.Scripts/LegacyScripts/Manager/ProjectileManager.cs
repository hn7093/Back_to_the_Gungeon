using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager instance;
    public static ProjectileManager Instance { get { return instance; } }

    [SerializeField] private GameObject[] projectilePrefabs;
    [SerializeField] private ParticleSystem impactParticleSystem;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // 총알 생성
    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPosition, Vector2 direction)
    {
        // 오브젝트 생성 후 초기화
        GameObject origin = projectilePrefabs[rangeWeaponHandler.BulletIndex];
        GameObject obj = Instantiate(origin, startPosition, Quaternion.identity);
        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController?.Init(direction, rangeWeaponHandler);
        // 반사, 관통 설정
        projectileController.SetBounce(rangeWeaponHandler.canBounce);
        projectileController.SetThrough(rangeWeaponHandler.canThrough);
    }

    // 파티클 재생
    public void CreateImpactParticlesAtPostion(Vector3 position, RangeWeaponHandler weaponHandler)
    {
        impactParticleSystem.transform.position = position;
        // 파티클 갯수 올림 처리 - 5배수
        ParticleSystem.EmissionModule em = impactParticleSystem.emission;
        em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(weaponHandler.BulletSize * 5)));
        // 파티클 속도 설정
        ParticleSystem.MainModule mainModule = impactParticleSystem.main;
        mainModule.startSpeedMultiplier = weaponHandler.BulletSize * 10f;
        // 재생
        impactParticleSystem.Play();
    }
}
