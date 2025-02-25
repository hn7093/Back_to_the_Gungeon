using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemyTest : MonoBehaviour
{
    [SerializeField] ResourceController controller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        controller = collision.gameObject.GetComponent<ResourceController>();
        controller.ChangeHealth(-10);
    }

}
