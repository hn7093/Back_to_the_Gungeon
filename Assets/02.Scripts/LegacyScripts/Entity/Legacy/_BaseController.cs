using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// 플레이어, 적 공통 클래스
public class _BaseController : MonoBehaviour
{
    public LayerMask enemyLayer; // 적 레이어 설정

    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] public WeaponHandler weaponPrefab;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }
    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } private set { lookDirection = value; } }
    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;
    private float timeSinceLastAttack = float.MaxValue;
    protected bool isAttacking;

    private List<BaseController> enemyList; // 적 리스트
    protected Transform targetEntity; // 타겟 엔티티
    // component
    protected Rigidbody2D _rigidbody;
    protected AnimationHandler _animationHandler;
    protected StatHandler _statHandler;
    protected WeaponHandler _weaponHandler;

    [SerializeField] WeaponSO weaponData;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animationHandler = gameObject.GetComponentInChildren<AnimationHandler>();
        _statHandler = GetComponent<StatHandler>();

        // 무기 찾기
        if (weaponPrefab != null)
        {
            _weaponHandler = Instantiate(weaponPrefab, weaponPivot);
        }
        else
        {
            _weaponHandler = GetComponentInChildren<WeaponHandler>();
        }
        // 무기 정보 적용 예시
        if (weaponData != null)
        {
            _weaponHandler.Setup(weaponData);
        }
    }
    protected virtual void Start()
    {

    }
    protected virtual void FixedUpdate()
    {
        // 이동
        Movement(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }
    protected virtual void Update()
    {
        HandleAction();

        // 회전
        Rotate(lookDirection);
        HandleAttackDelay();
    }

    protected virtual void HandleAction()
    {

    }
    private void Movement(Vector2 direction)
    {
        direction *= _statHandler.Speed;
        // 넉백이면 감속
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f;
            direction += knockback;
        }
        // 이동
        _rigidbody.velocity = direction;
        if (_animationHandler != null)
        {
            Debug.Log("not null");
            _animationHandler.Move(direction);
        }
        else
        {
            Debug.Log("null : " + gameObject.name);
        }
    }


    public void SetEnemyList(List<BaseController> enemies)
    {
        enemyList = enemies;
    }
    // 제일 가까운 타겟, 방향 찾기
    public bool SetCloserTarget()
    {
        if (enemyList == null || enemyList.Count == 0) return false;
        Debug.Log("SetCloserTarget : " + enemyList.Count);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (var enemy in enemyList)
        {
            // 활성화 된 오브젝트일 때만
            if (!enemy.gameObject.activeSelf) continue;

            // 비교용으로 차이의 제곱을 사용 - 제곱근 생략
            float dis = (enemy.transform.position - transform.position).sqrMagnitude;
            if (dis < closestDistance)
            {
                closestDistance = dis;
                closestEnemy = enemy.transform;
            }
        }
        if (closestEnemy == null)
        {
            return false;
        }
        else
        {
            LookDirection = closestEnemy.transform.position - transform.position;
            targetEntity = closestEnemy;
            return true;
        }
    }

    private void Rotate(Vector2 direction)
    {
        // 진행 방향에 따라 스프라이트의 보는 방향
        float rotz = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotz) > 90f;
        characterRenderer.flipX = isLeft;
        if (weaponPivot != null)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, rotz);
            weaponPivot.rotation = Quaternion.Lerp(weaponPivot.rotation, targetRotation, Time.deltaTime * 10f);
        }

        _weaponHandler?.Rotate(isLeft);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        // 역뱡향
        knockback = -(other.position - transform.position).normalized * power;
    }

    private void HandleAttackDelay()
    {
        if (_weaponHandler == null)
            return;

        if (timeSinceLastAttack <= _weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        // 공격 가능 여부 확인
        if (isAttacking && timeSinceLastAttack > _weaponHandler.Delay && SetCloserTarget())
        {
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    protected virtual void Attack()
    {
        _weaponHandler?.Attack();
    }

    public virtual void Death()
    {
        _rigidbody.velocity = Vector3.zero;
        // 모든 본인과 자식 스프라이트
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // 모든 본인과 자식 컴포넌트 비활성화
        foreach (Behaviour componet in transform.GetComponentsInChildren<Behaviour>())
        {
            componet.enabled = false;
        }

        // 2초후 삭제
        Destroy(gameObject, 2f);
    }
}
