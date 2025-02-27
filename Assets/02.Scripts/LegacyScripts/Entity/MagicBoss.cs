using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class MagicBoss : EnemyController
{
    [SerializeField] WeaponHandler[] weaponHandlers;
    private bool ready = false;
    private float lastAcitionTime = 0f;
    private float actionDuration = 2f; // 행동 지속 시간
    private NavMeshAgent agent;
    public void Init(Transform target)
    {
        closestEnemy = target;
        ready = true;
    }
    void Start()
    {
        closestEnemy = FindObjectOfType<PlayerController>().transform;
        lookDirection = (closestEnemy.position - transform.position).normalized;
        for (int i = 0; i < weaponHandlers.Length; i++)
        {
            WeaponSO weaponData = weaponHandlers[i].weaponData; // WeaponSO 가져오기
            weaponHandlers[i].Setup(weaponData);
        }
    }
    protected override void HandleAction()
    {
        //if (!ready) return;
        base.BaseHandleAction();
        lastAcitionTime += Time.deltaTime;
        if (lastAcitionTime > actionDuration)
        {
            lastAcitionTime = 0;
            // 랜덤 패턴
            int act = Random.Range(0, 4); // 0~3
            // 반복 이동
            switch (act)
            {
                case 0:
                    // 코투틴중에는 Dotween 사용불가로 미리 호출
                    _rigidbody.DOMove(-lookDirection, 3f);
                    StartCoroutine(Acition0());
                    break;
                case 1:
                    _rigidbody.DOMove(lookDirection, 3f);
                    StartCoroutine(Acition1());
                    break;
                case 2:
                    StartCoroutine(Acition2());
                    break;
                case 3:
                    _rigidbody.DOMove(-lookDirection, 3f).SetLoops(3, LoopType.Yoyo);
                    StartCoroutine(Acition3());
                    break;
            }
        }
    }

    // 공격 패턴
    protected virtual IEnumerator Acition0()
    {
        actionDuration = 5.0f;
        StartCoroutine(weaponHandlers[0].Attack());
        yield return new WaitForSeconds(1f);
        StartCoroutine(weaponHandlers[0].Attack());
        yield return new WaitForSeconds(1f);
        StartCoroutine(weaponHandlers[0].Attack());
    }
    protected virtual IEnumerator Acition1()
    {
        actionDuration = 5.0f;
        StartCoroutine(weaponHandlers[1].Attack());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(weaponHandlers[1].Attack());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(weaponHandlers[1].Attack());
    }
    protected virtual IEnumerator Acition2()
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
    protected virtual IEnumerator Acition3()
    {
        actionDuration = 7.0f;
        StartCoroutine(weaponHandlers[3].Attack());
        yield return new WaitForSeconds(4f);
        StartCoroutine(weaponHandlers[1].Attack());
    }

    public override void Death()
    {
        // 이미지, 충돌체 비활성화
        characterRenderer.enabled = false;
        Collider collider = GetComponentInChildren<Collider>();
        collider.enabled = false;
        base.Death();
    }
}
