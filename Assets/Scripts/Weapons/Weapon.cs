using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData weaponData;

    public virtual void Initialize(WeaponData data)
    {
        this.weaponData = data;
    }

    public abstract void Attack();
    public abstract void SpecialAttack();
    public abstract void Parry();
}
