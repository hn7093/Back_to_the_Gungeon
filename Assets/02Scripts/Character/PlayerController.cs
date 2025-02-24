using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class PlayerController : BaseController
{
    protected override void HandleAction()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

    }

    protected override void SetIsAttacking()
    {
        if(_rigidbody.velocity.magnitude == 0 )
            isAttacking = true;
        else
            isAttacking = false;
    }



}
