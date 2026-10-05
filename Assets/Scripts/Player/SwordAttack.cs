using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] PlayerStats pStats;
    private Rigidbody2D rb;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    GameManager gameManager;

    private void OnEnable()
    {
        gameManager = FindFirstObjectByType<GameManager>();


        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (gameManager.GameIsPaused) return;
        Attack();
    }

    void Attack()
    {
        if (gameManager.GameIsPaused) return;
        Debug.Log("Attacked");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        List<GameObject> enemies = new List<GameObject>();

        foreach (Collider2D enemy in hitEnemies)
        {
            if (!enemies.Contains(enemy.gameObject))
            {
                Debug.Log("We hit " + enemy.name);
                enemy.TryGetComponent<Enemy>(out Enemy enemyStats);
                enemy.TryGetComponent<EnemyKnockback>(out EnemyKnockback enemyKnockback);
                enemyStats.EnemyTakeDamage(pStats.swordDamage);
                enemyKnockback.Knockback(pStats.knockbackForce,pStats.knockbackTime, pStats.stunTime);
                enemies.Add(enemy.gameObject);
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        if (gameManager.GameIsPaused) return;
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
