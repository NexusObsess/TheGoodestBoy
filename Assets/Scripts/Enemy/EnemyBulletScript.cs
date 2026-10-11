using UnityEditor;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private GameObject player;

    private Rigidbody2D rb;
    public float force;
    private float timer;
    public float damage;
    public float knockbackForce;
    public float stunTime;

    GameManager gameManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);

        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GameIsPaused) return;
        timer = +Time.deltaTime;
        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.GameIsPaused) return;
        if (collision.CompareTag("Player"))
        {

            Debug.Log("Player Hit");
            collision.TryGetComponent<PlayerStats>(out PlayerStats pStats);
            pStats.PlayerTakeDamage(damage);
            collision.TryGetComponent<PlayerMovement>(out PlayerMovement pMove);
            pMove.Knockback(transform, knockbackForce, stunTime);
            Destroy(gameObject);
        }
    }

}
