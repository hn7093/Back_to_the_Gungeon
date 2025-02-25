using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Preference;
using UnityEngine;
using UnityEngine.Serialization;

public class AngelPage : UIMonoBehaviour
{
    public SkillUIElement[] skillElements;

    private void Start()
    {
        skillElements = transform.GetComponentsInChildren<SkillUIElement>();

        // 랜덤으로 가져오는 기능
        TextAsset jsonFile = Resources.Load<TextAsset>("Skill");
        SkillList skillList = JsonUtility.FromJson<SkillList>(jsonFile.text);
        List<SkillType> shuffledSkill = skillList.skills.OrderBy(x => Random.value).Take(skillElements.Length).ToList();
        
        for(int idx = 0; idx < skillElements.Length; idx++)
        {
            var currentElement = skillElements[idx];
            currentElement.SetData(shuffledSkill[idx]);
            currentElement.button.onClick.AddListener(() =>
            {
                Debug.Log(currentElement.id);
            });
        }
    }
}
