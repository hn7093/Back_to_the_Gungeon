using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game Data/WeaponUnlock")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public bool isUnlocked = false; // 해금 여부 저장
}