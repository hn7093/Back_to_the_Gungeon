using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseController : MonoBehaviour
{

    protected Rigidbody2D _rigidbody;

    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private SpriteRenderer rightHandRenderer;
    [SerializeField] private SpriteRenderer leftHandRenderer;
    [SerializeField] private SpriteRenderer weaponRenderer;
    [SerializeField] protected Transform rightHandPivot;
    [SerializeField] protected Transform leftHandPivot;
    [SerializeField] protected Transform weaponPivot;
    
    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }
    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }

    protected Vector2 weaponLookDirection = Vector2.zero;
    public Vector2 WeaponLookDirection { get { return lookDirection; } }
    public Transform closestEnemy;

    protected bool isLeft = false;

    protected AnimationHandler[] animationHandlers;

    protected float playerSpeed = 5f;
    protected float rotZ;
    protected Vector2 initialLeftHandPivotPos;
    protected Vector2 initialRightHandPivotPos;
    protected Vector2 initialWeaponPivotPos;


    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandlers = GetComponentsInChildren<AnimationHandler>(true);

        if (leftHandPivot != null)
            initialLeftHandPivotPos = leftHandPivot.localPosition;

        if (rightHandPivot != null)
            initialRightHandPivotPos = rightHandPivot.localPosition;

        if (weaponPivot != null)
            initialWeaponPivotPos = weaponPivot.localPosition;
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

    protected virtual void HandleAction()
    {

    }

    private void Movement(Vector2 direction)
    {
        direction = direction * playerSpeed;


        _rigidbody.velocity = direction;
        foreach (var handler in animationHandlers)
            if (handler != null)
                handler.Move(direction);
    }

    private void Rotate(bool _isLeft)
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
}
