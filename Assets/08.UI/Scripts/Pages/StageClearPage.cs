using System.Collections.Generic;
using Preference;
using TMPro;
using UnityEngine.UI;

// do: 로직이 완전히 같음
public class StageClearPage : UIMonoBehaviour
{
    public SkillUIElement[] skillElements;
    private List<Ability> shuffledSkill;
    public TMP_Text Description;
    
    private void OnDisable() { ShowShuffleAbilities(); }
    private void Start() { ShowShuffleAbilities(); }

    private void ShowShuffleAbilities()
    {
        Description.text = $"현재 스테이지: {systemManager.FileManager.CurrentStage.ToString()}";
        
        skillElements = transform.GetComponentsInChildren<SkillUIElement>();
        shuffledSkill = SystemManager.Instance.PlayerStatusManager.AbilityManager.GetShuffledAbilities(skillElements.Length);

        for(int idx = 0; idx < skillElements.Length; idx++)
        {
            var currentElement = skillElements[idx];
            currentElement.SetData(shuffledSkill[idx]);
            
            // 선택된 경우 Task로 전달
            currentElement.button.onClick.AddListener(() => { systemManager.EventManager.NotifyTaskComplete(currentElement.method); });
        }
    }
}
