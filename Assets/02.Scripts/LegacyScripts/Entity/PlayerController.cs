using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public enum controlType
{
    keyboard = 0,
    mouse
}

public class PlayerController : BaseController
{

    private List<BaseController> enemyList; // �� ����Ʈ

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isDragging = false;
    private float dragThreshold = 1f;
    private controlType currentControllType;
    private string controlTypeKey = "controlTypeKey";
    private bool isAnyEnemy = false;


    private void Start()
    {
        currentControllType = (controlType)PlayerPrefs.GetInt(controlTypeKey, 0);
    }

    protected override void Update()
    {
        base.Update();
        SetCloserTarget();
        HandleAttackDelay();
    }

    protected override void HandleAction()
    {
        if (currentControllType == 0)
            HandleKeyboardInput();
        else
            HandleMouseInput();
    }

    protected void HandleKeyboardInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            isDragging = true;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            currentTouchPosition = Input.mousePosition;
            float dragDistance = Vector2.Distance(startTouchPosition, currentTouchPosition);
            if (dragDistance > dragThreshold)
                movementDirection = (currentTouchPosition - startTouchPosition).normalized;
            else
                movementDirection = Vector2.zero;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            movementDirection = Vector2.zero;
        }
    }

    protected override void SetIsAttacking()
    {
        if (_rigidbody.velocity.magnitude == 0)
            isAttacking = true;
        else
            isAttacking = false;
    }

    public void SetEnemyList(List<BaseController> enemies)
    {
        enemyList = enemies;
    }

    public void SetCloserTarget()
    {
        if (enemyList == null || enemyList.Count == 0) isAnyEnemy = false;

        Debug.Log("SetCloserTarget : " + enemyList.Count);
        closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        float blockedDistance = Mathf.Infinity;
        Transform bestEnemy = null;
        Transform blockedEnemy = null;

        foreach (var enemy in enemyList)
        {
            // Ȱ��ȭ �� ������Ʈ�� ����
            if (!enemy.gameObject.activeSelf) continue;

            // �񱳿����� ������ ������ ��� - ������ ����
            float dis = (enemy.transform.position - weaponPivot.position).sqrMagnitude;
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(weaponPivot.position, directionToEnemy, Mathf.Sqrt(dis), LayerMask.GetMask("Wall", "innerWall"));

            if(hit.collider == null)//���� ������ bestEnemy�� ����
            {
                if(dis < closestDistance)
                {
                    closestDistance = dis;
                    bestEnemy = enemy.transform;
                }
            }
            else//���� ������ blockedEnemy�� ����
            {
                if (dis < closestDistance)
                {
                    blockedDistance = dis;
                    blockedEnemy = enemy.transform;
                }
            }
        }

        if (bestEnemy != null)
        {
            closestEnemy = bestEnemy;
            isAnyEnemy = true;
        }
        else if (blockedEnemy != null)
        {
            closestEnemy = blockedEnemy;
            isAnyEnemy = true;
        }
        else
            isAnyEnemy = false;
    }

    protected override void HandleAttackDelay()
    {
        if (_weaponHandler == null)
        {
            Debug.Log("weaponHandler is null");
            return;
        }

        if (timeSinceLastAttack <= _weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        // ���� ���� ���� Ȯ��
        if (isAttacking && timeSinceLastAttack > _weaponHandler.Delay && isAnyEnemy)
        {
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    public override void Death()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        animationHandlers[0].Die();

        // ��� ���ΰ� �ڽ� ������Ʈ ��Ȱ��ȭ
        StartCoroutine(DisableComponentsAfterDelay(1f));

        // ���ӿ��� ȭ�� ȣ��
    }

    private IEnumerator DisableComponentsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ��� ���ΰ� �ڽ� ������Ʈ ��Ȱ��ȭ
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }
    }
}
