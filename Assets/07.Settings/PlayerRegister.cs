using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;

public class PlayerRegister : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = transform.parent.gameObject;
        SystemManager.Instance.RegisterPlayer(player);
        Destroy(gameObject);
    }
}
