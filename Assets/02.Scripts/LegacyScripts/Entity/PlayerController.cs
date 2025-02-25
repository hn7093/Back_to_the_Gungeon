using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : BaseController
{

    private List<BaseController> enemyList; // �� ����Ʈ

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isDragging = false;
    private float dragThreshold = 1f;

    protected override void HandleAction()
    { HandleKeyboardInput(); }

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

    public bool SetCloserTarget()
    {
        if (enemyList == null || enemyList.Count == 0) return false;
        Debug.Log("SetCloserTarget : " + enemyList.Count);
        closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (var enemy in enemyList)
        {
            // Ȱ��ȭ �� ������Ʈ�� ����
            if (!enemy.gameObject.activeSelf) continue;

            // �񱳿����� ������ ������ ��� - ������ ����
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

    protected override void HandleAttackDelay()
    {
        if (_weaponHandler == null)
            return;

        if (timeSinceLastAttack <= _weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        // ���� ���� ���� Ȯ��
        if (isAttacking && timeSinceLastAttack > _weaponHandler.Delay && SetCloserTarget())
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
