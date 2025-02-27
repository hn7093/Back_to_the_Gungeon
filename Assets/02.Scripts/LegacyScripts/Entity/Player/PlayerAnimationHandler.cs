using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAnimationHandler : BaseAnimationHandler
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");
    private static readonly int IsAttack = Animator.StringToHash("IsAttack");
    private static readonly int IsDie = Animator.StringToHash("IsDie");

    public override void Move(Vector2 obj)
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

        //Debug.Log($" Move called with magnitude: {obj.magnitude}");

        animator.SetBool(IsMoving, obj.magnitude > 0.5f);
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
        animator.SetBool(IsAttack, true);
    }

    public override void Death()
    {
        Debug.Log("player dead");
        animator.SetTrigger(IsDie);
    }

    public override void InvincibilityEnd()
    {
        animator.SetBool(IsDamage, false);
    }

}
