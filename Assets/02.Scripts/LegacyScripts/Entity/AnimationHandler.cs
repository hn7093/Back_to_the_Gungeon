using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{


    private static readonly int IsMove = Animator.StringToHash("IsMove");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 obj)
    {

        if (animator == null)
        {
            Debug.LogWarning("Animator is null");
            return;
        }

        animator.SetBool(IsMove, obj.magnitude > .5f);//이동속도가  .5f 이상일때 이동애니메이션
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
