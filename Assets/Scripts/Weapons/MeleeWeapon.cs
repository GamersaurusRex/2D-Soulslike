using UnityEngine;

public class MeleeWeapon : Weapon
{
    public Animator animator;
    public float damage;
    public float range;
    public LayerMask enemyLayer;

    public override void Initialize(WeaponData data)
    {
        base.Initialize(data);
        damage = data.baseDamage;
        range = data.range;
        // Initialize other stats
    }

    public override void Attack()
    {       
        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

        // Apply damage to enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            //enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    public override void SpecialAttack()
    {
        
    }

    public override void Parry()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
