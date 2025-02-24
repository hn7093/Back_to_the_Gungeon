using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");
    protected Animator animator;
    protected virtual void Awake()
    {
        // 자신 포함 자식에서 가져오기
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMoving, obj.magnitude > 0.5f);
    }

    public void Damage()
    {
        animator.SetBool(IsDamage, true);
    }
    public void EndInvincibility()
    {
        animator.SetBool(IsDamage, false);
    }
}
