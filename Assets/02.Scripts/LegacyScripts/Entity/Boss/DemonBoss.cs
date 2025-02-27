using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class DemonBoss : EnemyController
{
    [SerializeField] WeaponHandler[] weaponHandlers;
    private bool ready = false;
    private float lastAcitionTime = 0f;
    private float actionDuration = 2f; // 행동 지속 시간
    private NavMeshAgent agent;
    private Animator _animator;
    //public override void Init(Transform target) 이게 옳지만 버그 이용함
    public void Init(Transform target)
    {
        closestEnemy = target;
        ready = true;
    }
    void Start()
    {
        closestEnemy = FindObjectOfType<PlayerController>().transform;
        closestEnemy.GetComponent<PlayerController>().SetEnemyList(new List<BaseController> { this });
        _animator = GetComponent<Animator>();
        lookDirection = (closestEnemy.position - transform.position).normalized;
        for (int i = 0; i < weaponHandlers.Length; i++)
        {
            WeaponSO weaponData = weaponHandlers[i].weaponData; // WeaponSO 가져오기
            weaponHandlers[i].Setup(weaponData);
        }
        ready = true;
    }
    protected override void HandleAction()
    {
        if (!ready) return;
        base.BaseHandleAction();
        lastAcitionTime += Time.deltaTime;
        if (lastAcitionTime > actionDuration)
        {
            lastAcitionTime = 0;
            _animator.SetBool("IsAttack", false);
            // 랜덤 패턴
            int act = Random.Range(0, 4); // 0~3
            // 반복 이동
            switch (act)
            {
                case 0:
                    // 코투틴중에는 Dotween 사용불가로 미리 호출
                    _rigidbody.DOMove(lookDirection, 4f);
                    StartCoroutine(Acition0());
                    break;
                case 1:
                    _rigidbody.DOMove(lookDirection, 4f);
                    StartCoroutine(Acition1());
                    break;
                case 2:
                    StartCoroutine(Acition2());
                    break;
                case 3:
                    actionDuration = 4.0f;
                    _animator.SetBool("IsAttack", true);
                    break;
            }
        }
    }

    // 공격 패턴
    protected IEnumerator Acition0()
    {
        actionDuration = 5.0f;
        StartCoroutine(weaponHandlers[0].Attack());
        yield return new WaitForSeconds(1f);
        StartCoroutine(weaponHandlers[0].Attack());
        yield return new WaitForSeconds(1f);
        StartCoroutine(weaponHandlers[0].Attack());
    }
    protected IEnumerator Acition1()
    {
        actionDuration = 5.0f;
        StartCoroutine(weaponHandlers[1].Attack());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(weaponHandlers[1].Attack());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(weaponHandlers[1].Attack());
    }
    protected IEnumerator Acition2()
    {
        actionDuration = 5.0f;
        weaponHandlers[2].SetBounce(true);
        StartCoroutine(weaponHandlers[2].Attack());
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(weaponHandlers[2].Attack());
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(weaponHandlers[2].Attack());
        weaponHandlers[2].SetBounce(false);
    }
    public void Acition4()
    {
        Debug.Log("Acition4");
        StartCoroutine(weaponHandlers[3].Attack());
    }

    public override void Death()
    {
        // 충돌체 비활성화
        Collider2D collider = GetComponentInChildren<Collider2D>();
        collider.enabled = false;
        // 2초 후 사망
        Invoke("DelayedDeath", 2f);
    }
    public void DelayedDeath()
    {
        base.Death();
    }
}
