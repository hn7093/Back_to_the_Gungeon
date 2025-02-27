using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;

public class NPC : MonoBehaviour
{

    public GameObject Player;

    // Awake 시 에러 남
    private void Start()
    {
        // SystemManager.Instance.RegisterPlayer(Player);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SystemManager.Instance.UIManager.OpenPage(PageType.IVENTORY_PAGE);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SystemManager.Instance.UIManager.OpenPage(PageType.PAUSE_PAGE);
        }
    }
    
    // NPC 또는 문을 통과할 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy(gameObject);
        SystemManager.Instance.AudioManager.PlayVFXSoundByName("click");
        
        SystemManager.Instance.EventManager.OpenEventPage(PageType.ANGEL_PAGE);
        // SystemManager.Instance.EventManager.OpenEventPage(PageType.DEVIL_PAGE);
        // SystemManager.Instance.EventManager.OpenEventPage(PageType.STAGE_CLEAR_PAGE);

        // SystemManager.Instance.EventManager.OpenEventPage(PageType.ROULETTE_PAGE);
    }
}
