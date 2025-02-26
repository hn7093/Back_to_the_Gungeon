using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Preference;
using UnityEngine;

public class AngelPage : UIMonoBehaviour
{
    public SkillUIElement[] skillElements;
    // do: 이벤트 매니저의 역할로 이동

    private void Start()
    {
        skillElements = transform.GetComponentsInChildren<SkillUIElement>();

        // 랜덤으로 섞어서 가져오는 기능(스킬 클래스 담당 기능)
        TextAsset jsonFile = Resources.Load<TextAsset>("Skill");
        SkillList skillList = JsonUtility.FromJson<SkillList>(jsonFile.text);
        List<SkillType> shuffledSkill = skillList.skills.OrderBy(x => Random.value).Take(skillElements.Length).ToList();
        
        for(int idx = 0; idx < skillElements.Length; idx++)
        {
            var currentElement = skillElements[idx];
            currentElement.SetData(shuffledSkill[idx]);
            // 선택된 경우 Task로 전달
            currentElement.button.onClick.AddListener(() => { systemManager.EventManager.NotifyTaskComplete(currentElement.id); });
        }
    }
}
