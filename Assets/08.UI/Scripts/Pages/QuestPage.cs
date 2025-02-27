using System;
using System.Collections;
using System.Collections.Generic;
using Preference;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPage : UIMonoBehaviour
{
    public TMP_Text quest1;
    public TMP_Text quest2;
    public Button ExitButton;
    
    private void OnEnable()
    {
        // Check Quest Complete
        if (SystemManager.Instance.EventManager.isQuest1Clear)
        {
            quest1.text = "고블린 몬스터 3마리 처리 - 완료";
            quest1.color = Color.blue;
        }
        
        if (SystemManager.Instance.EventManager.isQuest2Clear)
        {
            quest2.text = "박쥐 몬스터 3마리 처리 - 완료";
            quest2.color = Color.blue;
        }
    }

    private void Start()
    {
        ExitButton.onClick.AddListener(() => uiManager.Clear());
    }
}
