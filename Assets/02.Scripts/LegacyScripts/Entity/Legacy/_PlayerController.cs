using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class _PlayerController : _BaseController
{
    [Header("PlayerInfo")]
    private Camera camera;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        camera = Camera.main;
        isAttacking = true;
    }

    protected override void HandleAction()
    {

    }
    public override void Death()
    {
        base.Death();
        GameManager.Instance.GameOver();
    }

    void OnMove(InputValue inputValue)
    {
        movementDirection = inputValue.Get<Vector2>();
        movementDirection = movementDirection.normalized;

        // 눌림(이동중) = 공격 중지, 뗌(정지) = 공격
        if(movementDirection == Vector2.zero)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }
}
