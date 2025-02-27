using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponState
{
    public string name;
    public bool isLocked;

    public WeaponState(string _name)
    {
        name = _name;
        isLocked = true;
    }
} 

public class WeaponDatabase : MonoBehaviour
{
    public List<GameObject> weapons;
    private List<WeaponState> _weaponStates;

    private void Start()
    {
        foreach (var weapon in weapons)
        {
            _weaponStates.Add(new WeaponState(weapon.name));
        }
    }
}
