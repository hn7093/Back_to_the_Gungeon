using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemyTest : MonoBehaviour
{
    [SerializeField] ResourceController controller;
    [SerializeField] float damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        controller = collision.gameObject.GetComponent<ResourceController>();
        controller.ChangeHealth(damage*(-1f));
    }

}
