using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Preference
{
    public class Ability
    {
        public string Name;
        public string Description;
        public string Method;
        public string FilePath;
    }

    public enum SkillName
    {
        Double,
        Multi,
        Penetration,
        Reflection
    }
    
    public class AbilityDatabase: MonoBehaviour
    {
        public List<Ability> CurrentAbilities; 
        public List<SkillName> currentSkills;
    }
}