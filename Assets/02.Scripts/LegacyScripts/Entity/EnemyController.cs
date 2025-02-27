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
    [SerializeField] private float followRange = 20f; // 추적 거리
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float roamRadius = 3f; // 배회 범위
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool chase = true;
    [SerializeField] EnemyType enemyType = 0;
    
    protected EnemyManager enemyManager;
    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    public Transform AttackPos;
    private Vector2 lastPosition;
    protected bool isMove;
    private bool isPlayerInAttackRange = false;
    private ResourceController playerResourcController = null;

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
        _weaponHandler.Setup(weaponData);
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Movement();
    }

    protected void Movement()
    {
        if (enemyType == EnemyType.ranged) return;
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
        // 목표가 없거나, 무기가 없으면 행동 X
        if (_weaponHandler == null || target == null || canMove == false)
        {
            movementDirection = Vector2.zero;
            return;
        }

        // 행동 시작
        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        if (!isPlayerInAttackRange) // 🔥 플레이어가 공격 범위(Collide) 밖에 있을 때 추적
        {
            if (distance <= followRange) // 추적 가능 거리 안이면 이동
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
            else
            {
                agent.isStopped = true; // 너무 멀면 멈춤
            }
        }
        else // 🔥 공격 범위 안에 있을 때
        {
            agent.isStopped = true;
            agent.SetDestination(transform.position);

            if (isAttacking)
            {
                StartCoroutine(WaitAndAttack()); // 1초 대기 후 공격 실행
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어가 Collider 안에 들어오면 공격 모드
        {
            isPlayerInAttackRange = true;
            isAttacking = true;
            if (playerResourcController == null)
                playerResourcController = collision.GetComponent<ResourceController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어가 Collider 밖으로 나가면 추적 모드
        {
            isPlayerInAttackRange = false;
            isAttacking = false;
            if (playerResourcController != null)
                playerResourcController = null;
        }
    }

    private IEnumerator WaitAndAttack()
    {
        canMove = false;

        if (isPlayerInAttackRange && Time.time - lastAttackTime > attackCooldown) // 여전히 공격 범위 안이면 공격
        {
            StartCoroutine(AttackRoutine()); // 근접 공격 실행
            animationHandler.Attack(); // 애니메이션 실행
            lastAttackTime = Time.time; // 마지막 공격 시간 갱신
        }

        yield return new WaitForSeconds(1f); // 1초 대기

        canMove = true;

        if (isPlayerInAttackRange)
        {
            playerResourcController.ChangeHealth(_weaponHandler.Power * (-1f));
        }

    }


    private IEnumerator AttackRoutine()
    {
        if (_weaponHandler is MeleeWeaponHandler meleeWeapon)
        {
            yield return meleeWeapon.Attack(); // MeleeWeaponHandler의 Attack() 실행
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
        base.Death();
        EnemyManager.Instance.RemoveEnemyOnDeath(this);
    }
}
