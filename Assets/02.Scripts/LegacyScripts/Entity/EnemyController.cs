using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;


public enum EnemyType
{
    melee = 0,
    ranged,
    boss
}

// 적 행동 로직
public class EnemyController : BaseController
{
    [Header("EnemyInfo")]
    [SerializeField] private float followRange = 15f; // 추적 거리
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float roamRadius = 3f; // 배회 범위
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool chase = true;
    [SerializeField] EnemyType enemyType = 0;
    protected EnemyManager enemyManager;
    private Transform target;
    private NavMeshAgent agent;
    public Transform AttackPos;
    private Vector2 lastPosition;
    protected bool isMove;

    private Vector3 roamTarget; // 배회 위치
    private bool isRoaming; // 배회 중인지 여부
    private float attackCooldown = 1f; // 공격 쿨타임
    private float lastAttackTime;


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
        lastPosition = transform.position;
        closestEnemy = target;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Movement();
    }

    protected void Movement()
    {
        if(enemyType == EnemyType.ranged) return;
        //Debug.Log($" Move called with magnitude: {obj.magnitude}");

        Vector2 currentPosition = transform.position;
        isMove = (currentPosition != lastPosition);

        if (isMove)
            animationHandler.Move();
        else
            animationHandler.Stop();

        lastPosition = currentPosition;
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
        if (enemyType == EnemyType.melee)
            MeleeEnemyAction();
        else if (enemyType == EnemyType.ranged)
            RangedEnemyAction();
    }

    protected void MeleeEnemyAction()
    {
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
            if (canMove)
            {
                movementDirection = direction;
            }
        }
    }

    protected void RangedEnemyAction()
    {
        if (_weaponHandler == null || target == null)
        {
            if (!movementDirection.Equals(Vector2.zero))
            {
                movementDirection = Vector2.zero;
            }
            return;
        }

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        if (distance <= followRange)
        {
            // 플레이어 근처에서 배회 (Roam)
            if (!isRoaming && distance > attackRange)
            {
                isRoaming = true;
                roamTarget = GetRoamPosition();
                agent.SetDestination(roamTarget);
            }

            // 공격 가능 거리 안이라면 공격
            if (distance <= attackRange && Time.time - lastAttackTime > attackCooldown)
            {
                isAttacking = true;
                agent.SetDestination(transform.position); // 공격 시 멈추기
                StartCoroutine(AttackRoutine(direction));
                animationHandler.Attack();
                lastAttackTime = Time.time;
            }
            else
            {
                isAttacking = false;
            }
        }
    }

    private IEnumerator AttackRoutine(Vector2 direction)
    {
        if (_weaponHandler != null)
        {
            yield return _weaponHandler.Attack();
        }
    }

    private Vector3 GetRoamPosition()
    {
        // 플레이어 근처에서 랜덤 위치를 배회
        Vector3 randomDirection = (Random.insideUnitSphere * roamRadius);
        randomDirection += target.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, roamRadius, NavMesh.AllAreas);
        return navHit.position;
    }

    protected void BaseHandleAction()
    {
        base.HandleAction();
    }

    public override void Death()
    {
        animationHandler.Death();
        EnemyManager.Instance.RemoveEnemyOnDeath(this);
        base.Death();
    }
}
