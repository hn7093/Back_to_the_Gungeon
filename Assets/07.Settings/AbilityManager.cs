using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = Unity.Mathematics.Random;

namespace Preference
{
    [System.Serializable]
    public class Ability
    {
        public string name;
        public string description;
        public string method;
        public string filePath;
    }

    public class AbilityManager
    {
        public Queue<string> CurrentAbilities;
        
        private SkillList _skillList;
        private AbilityList _abilityList;

        public AbilityManager()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("data/abilityList");

            _skillList = JsonUtility.FromJson<SkillList>(jsonFile.text);
            _abilityList = JsonUtility.FromJson<AbilityList>(jsonFile.text);
            
        }

        public List<Ability> GetShuffledAbilities(int length)
        {
            return new List<Ability>(_skillList.skills)
                .Concat(_abilityList.abilities)
                .OrderBy(x => UnityEngine.Random.value)
                .Take(length)
                .ToList();
        }
    }
}