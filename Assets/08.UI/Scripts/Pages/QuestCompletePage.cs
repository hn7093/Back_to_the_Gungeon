using System;
using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;

public class QuestCompletePage : UIMonoBehaviour
{
    private void OnEnable()
    {
        // do: 애니메이션 doTween 느낌으로
        audioManager.PlayVFXSoundByName("victory");
        Invoke(nameof(DisablePage), 2f);
    }
    private void DisablePage()
    {
        gameObject.SetActive(false);
    }
}
