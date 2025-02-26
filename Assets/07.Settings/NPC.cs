using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private void Start()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SystemManager.Instance.RegisterPlayer(collision.gameObject);

        // SystemManager.Instance.EventManager.OpenEventPage(PageType.ANGEL_PAGE).ContinueWith(result =>
        // {
            // Debug.Log(result.Result);
        // });
    }
}
