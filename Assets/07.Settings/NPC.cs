using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;

public class NPC : MonoBehaviour
{

    // NPC 또는 문을 통과할 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
