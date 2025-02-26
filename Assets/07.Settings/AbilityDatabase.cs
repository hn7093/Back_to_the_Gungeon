using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Preference
{
    [Serializable]
    public class Ability
    {
        public string name;
        public string description;
        public string method;
        public string filePath;
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