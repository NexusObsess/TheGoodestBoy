using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    private GameObject player;
    public float damage;
    public float knockbackForce;
    public float stunTime;

    GameManager gameManager;

    void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        gameManager = FindFirstObjectByType<GameManager>();
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
        }
    }


}
