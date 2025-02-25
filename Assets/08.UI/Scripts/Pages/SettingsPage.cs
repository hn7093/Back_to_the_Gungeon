using System.Collections;
using System.Collections.Generic;
using Preference;
using UnityEngine;

public class SettingsPage : UIMonoBehaviour
{
    public void SetTurnBGM(bool state)
    {
        if (state) { systemManager.AudioManager._audioSource.Play(); }
        else { systemManager.AudioManager._audioSource.Stop(); }
    }
}
