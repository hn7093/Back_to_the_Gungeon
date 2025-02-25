using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{

    protected Rigidbody2D _rigidbody;

    [SerializeField] protected SpriteRenderer characterRenderer;
    [SerializeField] protected SpriteRenderer rightHandRenderer;
    [SerializeField] protected SpriteRenderer leftHandRenderer;
    [SerializeField] protected SpriteRenderer weaponRenderer;
    [SerializeField] public WeaponHandler weaponPrefab;
    [SerializeField] protected Transform rightHandPivot;
    [SerializeField] protected Transform leftHandPivot;
    [SerializeField] protected Transform weaponPivot;
    [SerializeField] protected WeaponSO weaponData;
    [SerializeField] protected float lookOffset = 1.5f;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }
    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } set { lookDirection = value; } }

    protected Vector2 weaponLookDirection = Vector2.zero;
    public Vector2 WeaponLookDirection { get { return weaponLookDirection; } }
    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;
    protected float timeSinceLastAttack = float.MaxValue;
    protected bool isAttacking = false;
    public bool IsAttacking { get { return isAttacking; } }
    public Transform closestEnemy;

    protected bool isLeft = false;
    protected Transform targetEntity; // 타겟 엔티티
    // component
    protected AnimationHandler animationHandler;

    protected StatHandler _statHandler;
    protected WeaponHandler _weaponHandler;
    protected float rotZ;
    protected Vector2 initialLeftHandPivotPos;
    protected Vector2 initialRightHandPivotPos;
    protected Vector2 initialWeaponPivotPos;


    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponentInChildren<AnimationHandler>();
        _statHandler = GetComponent<StatHandler>();

        if (leftHandPivot != null)
            initialLeftHandPivotPos = leftHandPivot.localPosition;

        if (rightHandPivot != null)
            initialRightHandPivotPos = rightHandPivot.localPosition;

        if (weaponPivot != null)
            initialWeaponPivotPos = weaponPivot.localPosition;

        // 무기 찾기
        if (weaponPrefab != null)
        {
            _weaponHandler = Instantiate(weaponPrefab, weaponPivot);
            Transform[] allChildren = weaponPivot.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {
                if (child.name == "WeaponSprite") //특정 이름과 일치하는 오브젝트 찾기
                {
                    weaponRenderer = child.GetComponent<SpriteRenderer>();
                    break;
                }
            }
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


    protected virtual void Update()
    {
        HandleAction();
        SetLookDirection();
        SetIsLeft();
        Rotate(isLeft);
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);


        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }

    protected virtual void SetLookDirection()
    {
        if (closestEnemy != null)
        {
            lookDirection = (closestEnemy.position - transform.position);

            if (lookDirection.magnitude < lookOffset && lookDirection.x >= 0)
                lookDirection = Vector3.right;
            else if (lookDirection.magnitude < lookOffset && lookDirection.x < 0)
                lookDirection = Vector3.left;

            //Debug.Log($"Look Direction: {lookDirection}, Closest Enemy: {closestEnemy.name}");
        }
        else
        {
            lookDirection = Vector2.zero;
        }

        if (weaponPivot != null)
        {
            if (closestEnemy != null)
            {
                weaponLookDirection = (closestEnemy.position - weaponPivot.position).normalized;

                if (lookDirection.magnitude < lookOffset && lookDirection.x >= 0)
                    weaponLookDirection = Vector3.right;
                else if (lookDirection.magnitude < lookOffset && lookDirection.x < 0)
                    weaponLookDirection = Vector3.left;

                //Debug.Log($"Look Direction: {weaponLookDirection}, Closest Enemy: {closestEnemy.name}");
            }
            else
            {
                weaponLookDirection = Vector2.zero;
            }
        }
        else
            return;

        rotZ = Mathf.Atan2(weaponLookDirection.y, weaponLookDirection.x) * Mathf.Rad2Deg;
    }

    protected virtual void SetIsAttacking() { }

    protected virtual void HandleAction()
    {

    }

    private void Movement(Vector2 direction)
    {
        if (_rigidbody == null) return;

        direction = direction * _statHandler.Speed;


        _rigidbody.velocity = direction;

        if (animationHandler != null)
            animationHandler.Move(direction);
        else
            Debug.Log("animationHandler is null");
    }

    private void Rotate(bool _isLeft)//무기 방향을 적에게 돌리고 적위치에 따라 좌우 반전
    {

        characterRenderer.flipX = _isLeft;

        if (leftHandRenderer != null)
            leftHandRenderer.flipX = _isLeft;

        if (rightHandRenderer != null)
            rightHandRenderer.flipX = _isLeft;

        if (weaponRenderer != null)
            weaponRenderer.flipY = _isLeft;


        if (leftHandPivot != null)
            RotatePivot(leftHandPivot, _isLeft, initialLeftHandPivotPos);

        if (rightHandPivot != null)
            RotatePivot(rightHandPivot, _isLeft, initialRightHandPivotPos);

        if (weaponPivot != null)
        {
            RotatePivot(weaponPivot, _isLeft, initialWeaponPivotPos);
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }

    private void RotatePivot(Transform pivot, bool _isLeft, Vector3 initialPos)
    {
        Vector3 localPos = pivot.localPosition;

        if (_isLeft)
        {
            localPos.x = -initialPos.x;
        }
        else
        {
            localPos.x = initialPos.x;
        }

        pivot.localPosition = localPos;
    }

    protected virtual void SetIsLeft()
    {

        if (lookDirection.x < 0)
            isLeft = true;
        else
            isLeft = false;
    }

    protected virtual void HandleAttackDelay() { }

    protected virtual void Attack()
    {
        _weaponHandler?.Attack();
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        // 역뱡향
        knockback = -(other.position - transform.position).normalized * power;
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
