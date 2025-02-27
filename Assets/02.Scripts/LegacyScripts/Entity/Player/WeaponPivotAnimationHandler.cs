using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPivotAnimationHandler : BaseAnimationController
{
    private static readonly int IsDie = Animator.StringToHash("IsDie");

    protected override void Awake()
    {
        base.Awake();
        if (animator == null)
            Debug.Log("no weaponpivot animator");

        animator.enabled = false;
    }

    public override void Init()
    {
        base.Init();
        animator.enabled = false;
    }
    public void Death()
    {
        StartCoroutine(DelayedSetTrigger(IsDie));
        //animator.enabled = false;
    }

    private IEnumerator DelayedSetTrigger(int key)
    {
        animator.enabled = true;
        yield return null;
        animator.SetTrigger(key);
    }
}
