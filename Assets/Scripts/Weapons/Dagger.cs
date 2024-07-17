using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MeleeWeapon
{
    public override void SpecialAttack()
    {
        
    }

    public override void Parry()
    {
        // Implement parry logic
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
