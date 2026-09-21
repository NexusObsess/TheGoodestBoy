using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] PlayerStats pStats;
    private Rigidbody2D rb;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;


    private void OnEnable()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        Attack();
    }

    void Attack()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.TryGetComponent<Enemy>(out Enemy enemyStats);
            enemyStats.EnemyTakeDamage(pStats.swordDamage);
        }

    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
