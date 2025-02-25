using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;


// 적 행동 로직
public class EnemyController : BaseController
{
    [Header("EnemyInfo")]
    [SerializeField] private float followRange = 15f; // 추적 거리
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool chase = true;
    private EnemyManager enemyManager;
    private Transform target;
    private NavMeshAgent agent;
    Animator animator;
    public Transform AttackPos;


    enum Enemy_State
    {
        Idle,
        Move,
        Attack,
        Damage,
        Death
    }

    public void Init(Transform target)
    {
        this.target = target;
 
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position);
    }
    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }
    protected override void HandleAction()
    {
        base.HandleAction();

        // 목표 없으면 행동 X
        if (_weaponHandler == null || target == null)
        {
            if (!movementDirection.Equals(Vector2.zero))
            {
                movementDirection = Vector2.zero;
            }
            return;
        }

        // 행동 시작
        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();
        //if (chase)
        {
            agent.SetDestination(target.position);
        }
        // 거리에 따라 공격 or 추격
        isAttacking = false;
        if (distance <= followRange)
        {
            // 방향 전환
            lookDirection = direction;
            if (chase)
            {
                agent.SetDestination(target.position);
            }
            // 공격 범위내라면
            if (distance < _weaponHandler.AttackRange)
            {
                // 물체 탐색 - Level 제외
                int layerMaskTarget = _weaponHandler.target;
                RaycastHit2D hit = Physics2D.Raycast(
                    transform.position,
                    direction,
                    _weaponHandler.AttackRange * 1.5f,
                    (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget
                    );
                // 물체 레이어로 공격 여부 결정
                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }
                // 이동 없이
                movementDirection = Vector2.zero;
                return;
            }

            // 이동
            if(canMove)
            {
                movementDirection = direction;
            }
        }
    }

    public override void Death()
    {
        base.Death();
        EnemyManager.Instance.RemoveEnemyOnDeath(this);
    }
}
