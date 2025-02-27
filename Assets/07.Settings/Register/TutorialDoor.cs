using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialDoor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SystemManager.Instance.UIManager.Clear();
            SceneManager.LoadScene("MainGame");
        }
    }
}
