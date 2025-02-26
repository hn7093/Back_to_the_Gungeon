using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;

public class NPC : MonoBehaviour
{

    public GameObject Player;

    // 플레이어 시작 부분에 추가 필요
    private void Awake()
    {
        SystemManager.Instance.RegisterPlayer(Player);
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
        SystemManager.Instance.EventManager.OpenEventPage(PageType.ANGEL_PAGE);
        // SystemManager.Instance.EventManager.OpenEventPage(PageType.DEVIL_PAGE);
        // SystemManager.Instance.EventManager.OpenEventPage(PageType.ROULETTE_PAGE);
    }
}
