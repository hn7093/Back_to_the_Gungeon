using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangeWeaponHandler : WeaponHandler
{

    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int bulletIndex;
    public int BulletIndex { get { return bulletIndex; } set => bulletIndex = value;}

    [SerializeField] private float bulletSize = 1;
    public float BulletSize { get { return bulletSize; } set => bulletSize = value;}

    [SerializeField] private float duration;
    public float Duration { get { return duration; } set => duration = value;} // 지속시간

    [SerializeField] private float spread;
    public float Spread { get { return spread; } set => spread = value;}

    [SerializeField] private int numberofProjectilesPerShot; // 발사당 갯수
    public int NumberofProjectilesPerShot { get { return numberofProjectilesPerShot; } set => numberofProjectilesPerShot = value;}

    [SerializeField] private float multipleProjectilesAngel; // 퍼짐 각도
    public float MultipleProjectilesAngel { get { return multipleProjectilesAngel; } set => multipleProjectilesAngel = value;}

    [SerializeField] private Color projectileColor;
    public Color ProjectileColor { get { return projectileColor; } set => projectileColor = value;}

    private ProjectileManager projectileManager;

    protected override void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        base.Attack();
        Debug.Log("Attack");
        float projectilesAngleSpace = multipleProjectilesAngel;
        int numberOfProjectilesPerShot = numberofProjectilesPerShot;

        // 발사 갯수에 따른 판 퍼짐 각도
        float minAngle = -(numberOfProjectilesPerShot / 2f) * projectilesAngleSpace + 0.5f * multipleProjectilesAngel;


        for (int i = 0; i < numberOfProjectilesPerShot; i++)
        {
            float angle = minAngle + projectilesAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;
            CreateProjectile(Controller.LookDirection, angle);
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
    }
}
