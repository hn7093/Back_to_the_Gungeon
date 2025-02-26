using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TempEnemyController : MonoBehaviour
{
    PlayerController player;
    List<BaseController> enemies;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();

        enemies = FindObjectsOfType<EnemyController>().Cast<BaseController>().ToList();
        if (player != null)
            player.SetEnemyList(enemies);
    }


}
