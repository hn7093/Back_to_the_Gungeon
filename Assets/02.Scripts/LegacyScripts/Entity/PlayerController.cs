using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class PlayerController : BaseController
{

    private List<BaseController> enemyList; // �� ����Ʈ
    protected override void HandleAction()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

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
