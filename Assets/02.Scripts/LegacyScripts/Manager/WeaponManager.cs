using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    [SerializeField] public List<WeaponUnlockData> allWeapons; // ��� ���� ������ ����
    private int currentWeaponIndex = 0;
    public int CurrentWeaponIndex { get => currentWeaponIndex; set { currentWeaponIndex = value; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        currentWeaponIndex = PlayerPrefs.GetInt(PlayerController.weaponIndexKey, 0);
    }

    public void UnlockWeapon(int index)
    {
        if (index >= 0 && index < allWeapons.Count)
        {
            allWeapons[index].isUnlocked = true;
            PlayerPrefs.SetInt("WeaponUnlocked_" + index, 1); // ����
        }
    }

    public bool IsWeaponUnlocked(int index)
    {
        return PlayerPrefs.GetInt("WeaponUnlocked_" + index, 0) == 1;
    }

    public WeaponUnlockData GetCurrentWeapon()
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

