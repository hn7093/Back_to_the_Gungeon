using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseController : MonoBehaviour
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
    [SerializeField] WeaponSO weaponData;

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
    protected Transform targetEntity; // Ÿ�� ��ƼƼ
    // component
    protected AnimationHandler[] animationHandlers;

    protected StatHandler _statHandler;
    protected WeaponHandler _weaponHandler;
    protected float rotZ;
    protected Vector2 initialLeftHandPivotPos;
    protected Vector2 initialRightHandPivotPos;
    protected Vector2 initialWeaponPivotPos;


    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandlers = GetComponentsInChildren<AnimationHandler>(true);
        _statHandler = GetComponent<StatHandler>();

        if (leftHandPivot != null)
            initialLeftHandPivotPos = leftHandPivot.localPosition;

        if (rightHandPivot != null)
            initialRightHandPivotPos = rightHandPivot.localPosition;

        if (weaponPivot != null)
            initialWeaponPivotPos = weaponPivot.localPosition;

        // ���� ã��
        if (weaponPrefab != null)
        {
            _weaponHandler = Instantiate(weaponPrefab, weaponPivot);
        }
        else
        {
            _weaponHandler = GetComponentInChildren<WeaponHandler>();
        }
        // ���� ���� ���� ����
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
            lookDirection = (closestEnemy.position - transform.position).normalized;
            Debug.Log($"Look Direction: {lookDirection}, Closest Enemy: {closestEnemy.name}");
        }
        else
        {
            lookDirection = Vector2.zero;
        }

        if (closestEnemy != null)
        {
            weaponLookDirection = (closestEnemy.position - weaponPivot.position).normalized;
            Debug.Log($"Look Direction: {weaponLookDirection}, Closest Enemy: {closestEnemy.name}");
        }
        else
        {
            weaponLookDirection = Vector2.zero;
        }


        rotZ = Mathf.Atan2(weaponLookDirection.y, weaponLookDirection.x) * Mathf.Rad2Deg;
    }

    protected virtual void SetIsAttacking() { }

    protected virtual void HandleAction()
    {

    }

    private void Movement(Vector2 direction)
    {
        direction = direction * _statHandler.Speed;


        _rigidbody.velocity = direction;
        foreach (var handler in animationHandlers)
            if (handler != null)
                handler.Move(direction);
    }

    private void Rotate(bool _isLeft)//���� ������ ������ ������ ����ġ�� ���� �¿� ����
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
        // ������
        knockback = -(other.position - transform.position).normalized * power;
    }

    public virtual void Death()
    {
        _rigidbody.velocity = Vector3.zero;
        // ��� ���ΰ� �ڽ� ��������Ʈ
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // ��� ���ΰ� �ڽ� ������Ʈ ��Ȱ��ȭ
        foreach (Behaviour componet in transform.GetComponentsInChildren<Behaviour>())
        {
            componet.enabled = false;
        }

        // 2���� ����
        Destroy(gameObject, 2f);
    }
}
