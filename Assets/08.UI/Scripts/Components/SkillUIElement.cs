using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Preference
{
    public class SkillUIElement: MonoBehaviour
    {
        public string id;

        [HideInInspector] 
        public Button button;
        
        public Image image;           
        public TMP_Text title;
        public TMP_Text description;

        private void Awake()
        {
            button = GetComponent<Button>();
        }
        
        public void SetData(SkillType skill)
        {
            id = skill.id;
            image.sprite = Resources.Load<Sprite>(skill.imagePath);
            title.text = skill.name;
            description.text = skill.description;
        }
    }
}