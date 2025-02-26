using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Preference
{
    public class SkillUIElement: MonoBehaviour
    {
        public string method;

        [HideInInspector] 
        public Button button;
        
        public Image image;           
        public TMP_Text title;
        public TMP_Text description;

        private void Awake()
        {
            button = GetComponent<Button>();
        }
        
        public void SetData(Ability skill)
        {
            method = skill.method;
            image.sprite = Resources.Load<Sprite>(skill.filePath);
            title.text = skill.name;
            description.text = skill.description;
        }
    }
}