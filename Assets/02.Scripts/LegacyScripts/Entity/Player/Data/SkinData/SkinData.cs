using UnityEngine;

[CreateAssetMenu(fileName = "NewSkin", menuName = "Game Data/Skin")]
public class SkinData : ScriptableObject
{
    public string skinName;
    public GameObject skinPrefab;
    public bool isUnlocked = false; // 해금 여부 저장
}
