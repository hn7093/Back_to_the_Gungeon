using System;
using Preference;
using UnityEngine;

public class NPCRegister : MonoBehaviour
{
    public enum NPCType { ANGEL, DEVIL }
    
    public NPCType npcType;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (npcType == NPCType.ANGEL)
            {
                SystemManager.Instance.EventManager.OpenEventPage(PageType.ANGEL_PAGE).ContinueWith(_ =>
                {
                    Destroy(gameObject);
                });
                return;
            }

            if (npcType == NPCType.DEVIL)
            {
                SystemManager.Instance.EventManager.OpenEventPage(PageType.DEVIL_PAGE).ContinueWith(_ =>
                {
                    Destroy(gameObject);
                });
            }
        }
    }
}
