using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangeWeaponHandler : WeaponHandler
{

    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    private int bulletIndex; // 탄알 인덱스
    public int BulletIndex { get { return bulletIndex; } set => bulletIndex = value; }

    private float bulletSize = 1; // 탄알 크기
    public float BulletSize { get { return bulletSize; } set => bulletSize = value; }

    private float duration; // 지속시간
    public float Duration { get { return duration; } set => duration = value; } // 지속시간

    private float spread; // 탄 퍼짐
    public float Spread { get { return spread; } set => spread = value; }

    private int numberofProjectilesPerShot; // 발사당 갯수
    public int NumberofProjectilesPerShot { get { return numberofProjectilesPerShot; } set => numberofProjectilesPerShot = value; }

    private float multipleProjectilesAngel; // 여러개 발사시 최소 각도 차이
    public float MultipleProjectilesAngel { get { return multipleProjectilesAngel; } set => multipleProjectilesAngel = value; }
    private float multipleDelay; // 여러개 발사시 대기 딜레이
    public float MultipleDelay { get { return multipleDelay; } set => multipleDelay = value; }
    private Color projectileColor;
    public Color ProjectileColor { get { return projectileColor; } set => projectileColor = value; }

    // 관통
    public bool canBounce = false;
    public bool canThrough = false;

    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator Attack()
    {
        StartCoroutine(base.Attack());
        // 발사 갯수에 따른 판 퍼짐 각도
        float minAngle = -(numberofProjectilesPerShot / 2f) * multipleProjectilesAngel + 0.5f * multipleProjectilesAngel;

        for (int i = 0; i < numberofProjectilesPerShot; i++)
        {
            float angle = minAngle + multipleProjectilesAngel * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;
            CreateProjectile(Controller.LookDirection, angle);
            yield return new WaitForSeconds(multipleDelay);
        }
    }

    private void CreateProjectile(Vector2 _lookDirection, float angle)
    {
        ProjectileManager.Instance.ShootBullet(
            this, projectileSpawnPosition.position,
            RotateVector2(_lookDirection, angle)
            );
    }
    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
    public override void Setup(WeaponSO weaponData)
    {
        base.Setup(weaponData);
        AttackRange = weaponData.attackRange;
        BulletIndex = weaponData.bulletIndex;
        BulletSize = weaponData.bulletSize;
        Duration = weaponData.duration;
        Spread = weaponData.spread;
        NumberofProjectilesPerShot = weaponData.numberofProjectilesPerShot;
        MultipleProjectilesAngel = weaponData.multipleProjectilesAngel;
        ProjectileColor = weaponData.projectileColor;
        MultipleDelay = weaponData.multipleDelay;
    }

    // 발사수 증가
    public override void AddFrontBullet(int plus)
    {
        NumberofProjectilesPerShot += plus;
        // 발당 딜레이 설정
        if (MultipleDelay == 0)
        {
            MultipleDelay = 0.1f;
        }
    }
    // 반사 설정
    public override void SetBounce(bool canBounce)
    {
        this.canBounce = canBounce;
    }
    // 관통 설정
    public override void SetThrough(bool canThrough)
    {
        this.canThrough = canThrough;
    }
    // 이어서 쏘는 경우 산탄으로 변경
    public override void SetSpread()
    {
        MultipleDelay = 0.0f;
        MultipleProjectilesAngel = 10.0f;
    }

    // 산탄으로 쏘는 경우 이어서 쏘도록 변경
    public override void SetFocus()
    {
        MultipleDelay = 0.1f;
        MultipleProjectilesAngel = 0.0f;
    }
}
