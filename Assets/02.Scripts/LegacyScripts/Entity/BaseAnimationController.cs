using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAnimationController : MonoBehaviour
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
}
