using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");
    private static readonly int IsAttack = Animator.StringToHash("IsAttack");
    private static readonly int IsDeath = Animator.StringToHash("IsDeath");

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMoving, obj.magnitude > 0.5f);//이동속도가  0.5f 이상일때 이동애니메이션
    }

    public void EndInvincibility()
    {
        animator.ResetTrigger(IsDamage);
    }
    public void Damage()
    {
        animator.SetBool(IsDamage, true);
    }

    public void Attack()
    {
        animator.SetBool(IsAttack, true);
    }

    public void Death()
    {
        animator.SetTrigger(IsDeath);
    }

    public void InvincibilityEnd()
    {
        animator.SetBool(IsDamage, false);
    }
}
