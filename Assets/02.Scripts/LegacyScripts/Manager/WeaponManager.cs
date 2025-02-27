using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    [SerializeField] public List<WeaponData> allWeapons; // 모든 무기 데이터 저장
    private int currentWeaponIndex = 0;
    public int CurrentWeaponIndex { get => currentWeaponIndex; set { currentWeaponIndex = value; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        currentWeaponIndex = PlayerPrefs.GetInt(PlayerController.weaponIndexKey, 0);
    }

    public void UnlockWeapon(int index)
    {
        if (index >= 0 && index < allWeapons.Count)
        {
            allWeapons[index].isUnlocked = true;
            PlayerPrefs.SetInt("WeaponUnlocked_" + index, 1); // 저장
        }
    }

    public bool IsWeaponUnlocked(int index)
    {
        return PlayerPrefs.GetInt("WeaponUnlocked_" + index, 0) == 1;
    }

    public WeaponData GetCurrentWeapon()
    {
        return allWeapons[currentWeaponIndex];
    }

    public void SetWeapon(int index)
    {
        if (index >= 0 && index < allWeapons.Count && IsWeaponUnlocked(index))
        {
            currentWeaponIndex = index;
            PlayerPrefs.SetInt("SelectedWeapon", index);
        }
    }
}

