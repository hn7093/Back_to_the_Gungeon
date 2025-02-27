using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;

public class SecondRegister : MonoBehaviour
{
    private async void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            
            playerController.canMove = false;
            
            await SystemManager.Instance.EventManager.OpenEventPage(PageType.ANGEL_PAGE);
            
            SystemManager.Instance.EventManager.OpenEventPage(PageType.TUTORIAL_PAGE);
            SystemManager.Instance.EventManager.isTutorial2Clear = true;

            if (gameObject) Destroy(gameObject);
            playerController.canMove = true;
        }
    }
}
