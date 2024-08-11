using UnityEngine;

public class WeaponFactory : MonoBehaviour
{
    public static Weapon CreateWeapon(WeaponData data)
    {
        GameObject weaponObject = new(data.weaponName);
        Weapon weapon = null;

        switch (data.weaponName)
        {
            case "Dagger":
                weapon = weaponObject.AddComponent<Dagger>();
                break;
            //case "Sword":
            //    weapon = weaponObject.AddComponent<Sword>();
            //    break;
        }

        if (weapon != null)
        {
            weapon.Initialize(data);
        }

        return weapon;
    }
}
