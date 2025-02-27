using System;
using Preference;
using UnityEngine;

public class NPCRegister : MonoBehaviour
{
    public enum NPCType { ANGEL, DEVIL }
    
    public NPCType npcType;

    private async void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (npcType == NPCType.ANGEL)
            {
                await SystemManager.Instance.EventManager.OpenEventPage(PageType.ANGEL_PAGE);
                if(gameObject) Destroy(gameObject);
            }

            if (npcType == NPCType.DEVIL)
            {
                await SystemManager.Instance.EventManager.OpenEventPage(PageType.DEVIL_PAGE);
                if(gameObject) Destroy(gameObject);
            }
        }
    }
}
