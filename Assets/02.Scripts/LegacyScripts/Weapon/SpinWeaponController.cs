using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWeaponController : MonoBehaviour
{
    private SpinWeaponHandler _spinWeaponHandler;
    public float scale = 1.0f; // 데미지 배율
    // 초기화
    public void Init(SpinWeaponHandler spinWeaponHandler)
    {
        _spinWeaponHandler = spinWeaponHandler;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // target은 rangeWeaponHandler의 레이어
        if (_spinWeaponHandler.target.value == (_spinWeaponHandler.target.value | (1 << collision.gameObject.layer)))
        {
            // 목표 오브젝트
            // 데미지 & 넉백
            ResourceController resourceController = collision.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                // 배율 적용 데미지
                resourceController.ChangeHealth(-_spinWeaponHandler.Power * scale);
                // 넉백
                if (_spinWeaponHandler.IsOnKnockback)
                {
                    BaseController controller = collision.GetComponent<BaseController>();
                    if (controller != null)
                    {
                        controller.ApplyKnockback(transform, _spinWeaponHandler.KnockbackPower, _spinWeaponHandler.KnockbackTime);
                    }
                }
            }
        }
    }
}
