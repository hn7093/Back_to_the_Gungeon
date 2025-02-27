using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game Data/WeaponUnlock")]
public class WeaponUnlockData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public bool isUnlocked = false; // 해금 여부 저장
}