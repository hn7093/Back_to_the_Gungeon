using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosslineUI : MonoBehaviour
{
    PlayerController player;
    [Range(0.01f, 1.5f)][SerializeField] private float smoothSpeed = 0.25f;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("PlayerController is null");
            return;
        }

        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if(IsAnyEnemy()) 
            TrackOnTarget();
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    private void TrackOnTarget()
    {
        if (player == null) return;

        Vector3 desirePosition;
        desirePosition = Camera.main.WorldToScreenPoint(player.closestEnemy.position);
        transform.position = Vector3.Lerp(transform.position, desirePosition, smoothSpeed);

    }

    private bool IsAnyEnemy()
    {
        if (player == null) 
        { 
            Debug.LogError("PlayerController is null"); 
            return false; 
        }

        if (player.closestEnemy != null)
        {
            Debug.Log($"{player.closestEnemy.name} is targeting");
            return true;
        }
        else
        {
            Debug.Log("enemy is null");
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
            return false;

        }
    }
}
