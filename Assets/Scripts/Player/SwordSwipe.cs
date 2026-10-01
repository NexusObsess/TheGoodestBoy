using UnityEngine;

public class SwordSwipe : MonoBehaviour
{
    public GameController gameController;
    private Rigidbody2D rb;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GameIsPaused) return;
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

    }

    void Attack()
    {
        if (gameManager.GameIsPaused) return;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);

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
