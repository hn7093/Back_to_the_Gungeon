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
        Init();
    }

    public void Init()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 obj)
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

    public void EndInvincibility()
    {
        animator.ResetTrigger(IsDamage);
    }
    public void Damage()
    {
        animator.SetTrigger(IsDamage);
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
