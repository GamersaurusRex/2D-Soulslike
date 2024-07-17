using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public bool isTwoHanded;
    public string weaponType;

    public float baseDamage;
    public float range;
    public float attackCooldownTime;
    public float comboCooldownTime;
    public int comboCount;

    public float attackStaminaCost;
    public float attackManaCost;

    public float parryWindow;
    public float parryStaminaCost;
    public float blockStaminaCost;

    public string specialAbility;
    public float manaCostForSpecial;
    public float cooldownTimeForSpecial;

    public float weight;
    public float durability;
    public float knockback;

    public float criticalHitChance;
    public float criticalHitMultiplier;
    public float attackSpeedModifier;
    public string statusEffect; 
    public Vector2 hitboxSize;
}
