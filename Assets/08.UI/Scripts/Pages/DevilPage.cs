using System;
using System.Collections.Generic;
using Preference;
using UnityEngine.UI;

public class DevilPage : UIMonoBehaviour
{
    public Button ExitButton;
    
    // 보상 로직 동일함.
    public SkillUIElement[] skillElements;
    private List<Ability> shuffledSkill;
    
    private void OnDisable() { ShowShuffleAbilities(); }
    private void Start() { ShowShuffleAbilities(); }
    private void OnEnable() { audioManager.PlayVFXSoundByName("devil"); }

    private void ShowShuffleAbilities()
    {
        skillElements = transform.GetComponentsInChildren<SkillUIElement>();
        shuffledSkill = SystemManager.Instance.PlayerStatusManager.AbilityManager.GetShuffledAbilities(skillElements.Length);

        for(int idx = 0; idx < skillElements.Length; idx++)
        {
            var currentElement = skillElements[idx];
            currentElement.SetData(shuffledSkill[idx]);
            
            // 선택된 경우 Task로 전달
            currentElement.button.onClick.AddListener(() =>
            {
                // 체력 감소를 보상으로 스킬 획득
                systemManager.PlayerStatusManager.DecreaseCurrentHealth();
                systemManager.EventManager.NotifyTaskComplete(currentElement.method);
            });
        }
        
        ExitButton.onClick.AddListener(() => { systemManager.EventManager.NotifyTaskComplete(null); });
    }
}
