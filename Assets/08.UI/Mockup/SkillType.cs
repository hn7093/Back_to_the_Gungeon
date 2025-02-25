using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SkillType
{
    public string id;
    public string name;
    public string description;
    public string imagePath;
}

[Serializable]
public class SkillList  // JSON 배열을 감싸는 클래스
{
    public List<SkillType> skills;
}