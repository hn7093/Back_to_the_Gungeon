using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosslineUI : MonoBehaviour
{
    PlayerController player;
    [Range(0.01f, 1.5f)][SerializeField] private float smoothSpeed = 0.25f;
    [SerializeField] private GameObject crosslineIMG;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("PlayerController is null");
            return;
        }

        if (crosslineIMG == null)
        crosslineIMG = GetComponentInChildren<GameObject>();

        crosslineIMG.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if(IsAnyEnemy()) 
            TrackOnTarget();
    }

    public void SetActiveCrossline()
    {
        crosslineIMG.gameObject.SetActive(true);
    }

    public void SetActiveCrosslineFalse()
    {
        crosslineIMG.gameObject.SetActive(false);
    }

    private void TrackOnTarget()
    {
        if (player == null) return;

        Vector3 desirePosition;
        desirePosition = Camera.main.WorldToScreenPoint(player.closestEnemy.position);
        crosslineIMG.transform.position = Vector3.Lerp(transform.position, desirePosition, smoothSpeed);

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
            if (crosslineIMG.gameObject.activeSelf)
                SetActiveCrosslineFalse();
            return false;
        }
    }
}
