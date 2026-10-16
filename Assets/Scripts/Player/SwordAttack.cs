using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] PlayerStats pStats;
    private Rigidbody2D rb;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    [Header ("Sound Effects")]
    public AudioSource soundSource;
    public AudioClip swordSwingSound;
    public AudioClip dogBarkSound;

    public float pitchVariation = 0.15f;

    GameManager gameManager;

    PlayerMovement playerMovement;

    private void OnEnable()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerMovement = GetComponentInParent<PlayerMovement>();


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
        PlayAudio(dogBarkSound);
        PlayAudio(swordSwingSound);

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


    public void PlayAudio(AudioClip toPlay)
    {
        soundSource.clip = toPlay;
        if(soundSource != null && soundSource.clip != null)
        {
            float randomPitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
            soundSource.pitch = randomPitch;
            soundSource.PlayOneShot(soundSource.clip);
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
