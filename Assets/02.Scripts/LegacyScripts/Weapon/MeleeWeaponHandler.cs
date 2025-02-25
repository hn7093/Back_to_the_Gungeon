using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHandler : WeaponHandler
{
    [Header("Melee Attack Info")]
    public Vector2 collideBoxSize = Vector2.one;
    public Vector2 CollideBoxSize { get => collideBoxSize; set => collideBoxSize = value; }

    protected override void Start()
    {
        base.Start();
        collideBoxSize = collideBoxSize * WeaponSize;
    }

    public override void Attack()
    {
        base.Attack();
        // 사각형에 포함되는지 검사
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position + (Vector3)Controller.LookDirection * collideBoxSize.x,
            collideBoxSize,
            0,
            Vector2.zero,
            0,
            target);

        if(hit.collider != null)
        {
            ResourceController resourceController = hit.collider.GetComponent<ResourceController>();
            if(resourceController != null)
            {
                // 데미지
                resourceController.ChangeHealth(-Power);
                // 넉백
                if(IsOnKnockback)
                {
                    BaseController controller = hit.collider.GetComponent<BaseController>();
                    if(controller != null)
                    {
                        controller.ApplyKnockback(transform, KnockbackPower, KnockbackTime);
                    }
                }
            }
        }
    }

    public override void Rotate(bool isLeft)
    {
        // 근접 무기는 원형이 아니라 좌우반전만
        if(isLeft)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public override void Setup(WeaponSO weaponData)
    {
        base.Setup(weaponData);
        CollideBoxSize = weaponData.collideBoxSize;
    }
}
