using System.Collections.Generic;
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
        Debug.Log("Attacked");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        List<GameObject> enemies = new List<GameObject>();

        foreach (Collider2D enemy in hitEnemies)
        {
            if (!enemies.Contains(enemy.gameObject))
            {
                Debug.Log("We hit " + enemy.name);
                enemy.TryGetComponent<Enemy>(out Enemy enemyStats);
                enemyStats.EnemyTakeDamage(pStats.swordDamage);
                enemies.Add(enemy.gameObject);
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
