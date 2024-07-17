using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<WeaponData> weaponDataList;

    private void Start()
    {
        weaponDataList = new List<WeaponData>();
    }

    public void AddWeapon(WeaponData weaponData)
    {
        weaponDataList.Add(weaponData);
    }

    public Weapon EquipWeapon(string weaponName)
    {
        WeaponData data = weaponDataList.Find(w => w.weaponName == weaponName);
        if (data != null)
        {
            return WeaponFactory.CreateWeapon(data);
        }
        return null;
    }
}
