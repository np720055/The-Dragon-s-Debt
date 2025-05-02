using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;

    public int lightAttackDamage = 10;
    public int heavyAttackDamage = 20;

    public enum AttackType { Light, Heavy }

    public void DealDamage(AttackType type)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (var enemy in hitEnemies)
        {
            int damage = (type == AttackType.Heavy) ? heavyAttackDamage : lightAttackDamage;

            SkeletonAI skeleton = enemy.GetComponent<SkeletonAI>();
            if (skeleton != null)
            {
                skeleton.TakeDamage(damage);
                Debug.Log($"Dealt {damage} damage to Skeleton.");
                continue;
            }

            HealthSystem health = enemy.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log($"Dealt {damage} damage to Vortemar ");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
