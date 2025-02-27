using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHandler : BaseAnimationHandler
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");
    private static readonly int IsAttack = Animator.StringToHash("IsAttack");
    private static readonly int IsDeath = Animator.StringToHash("IsDeath");

    Vector2 lastPosition;

    public override void Move()
    {
        if (animator == null)
        {
            Debug.LogError(" Animator is NULL in AnimationHandler!");
            return;
        }

        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError(" AnimatorController is not assigned to Animator!");
            return;
        }

        // 애니메이터 파라미터 설정
        animator.SetBool(IsMoving, true);
    }

    public override void Stop()
    {
        animator.SetBool(IsMoving, false);
    }


    public override void EndInvincibility()
    {
        animator.ResetTrigger(IsDamage);
    }
    public override void Damage()
    {
        animator.SetTrigger(IsDamage);
    }

    public override void Attack()
    {
        animator.SetTrigger(IsAttack);
    }

    public override void Death()
    {
        animator.SetTrigger(IsDeath);
    }

    public override void InvincibilityEnd()
    {
        animator.ResetTrigger(IsDamage);
    }
}
