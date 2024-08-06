using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MeleeWeapon
{
    public override void Attack()
    {
        // Play attack animation
        animator.SetTrigger("DaggerAttack");
        base.Attack();
    }

    public override void SpecialAttack()
    {
        
    }

    public override void Parry()
    {
        // Implement parry logic
    }
}
