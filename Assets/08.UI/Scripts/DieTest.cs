using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;

public class DieTest : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // SystemManager.Instance.UIManager.OpenPage(PageType.QUEST_COMPLETE_PAGE);   
        Destroy(gameObject);
    }
}
