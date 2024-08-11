using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory playerInventory;
    public WeaponData daggerData;
    public WeaponData swordData;

    private void Start()
    {
        // Add weapon data to inventory
        playerInventory.AddWeapon(daggerData);
        playerInventory.AddWeapon(swordData);

        // Equip a weapon
        Weapon equippedWeapon = playerInventory.EquipWeapon("Dagger");
    }
}
