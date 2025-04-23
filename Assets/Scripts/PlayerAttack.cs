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
            SkeletonAI skeleton = enemy.GetComponent<SkeletonAI>();
            if (skeleton != null)
            {
                int damage = (type == AttackType.Heavy) ? heavyAttackDamage : lightAttackDamage;
                skeleton.TakeDamage(damage);
                Debug.Log($"Dealt {damage} damage to skeleton.");
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
