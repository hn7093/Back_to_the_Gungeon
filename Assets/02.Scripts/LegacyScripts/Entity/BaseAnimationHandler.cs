using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class BaseAnimationHandler : MonoBehaviour
{
    protected Animator animator;


    protected virtual void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        animator = GetComponentInChildren<Animator>();
    }


    public virtual void Move() { }

    public virtual void Move(Vector2 vector2) { }

    public virtual void Stop() { }
    public virtual void EndInvincibility() { }
    public virtual void Attack() { }


    public virtual void Damage() { }
    public virtual void Death() { }

    public virtual void InvincibilityEnd() { }

    
   
}
