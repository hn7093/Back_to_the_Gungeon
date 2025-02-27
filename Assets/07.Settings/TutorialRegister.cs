using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;

public class TutorialRegister : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(StartTutorialWithDelay(0.5f));
    }

    IEnumerator StartTutorialWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SystemManager.Instance.EventManager.TutorialStart();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SystemManager.Instance.EventManager.isTutorial1Clear = true;
            Destroy(gameObject);
        }
    }
}
