using UnityEngine;
using System;
using System.Collections.Generic;
using Preference;

[Serializable]
public class SkillList  // JSON 배열을 감싸는 클래스
{
    public List<Ability> skills;
}

[Serializable]
public class AbilityList
{
    public List<Ability> abilities;
}