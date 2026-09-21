using UnityEngine;
using System.Collections;
public class Enemy : MonoBehaviour
{
    public float hp = 5f;
    public float speed = 2.5f;
    public float cooldown;
    public float damage = 1f;

    private SpriteRenderer sRenderer;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;

    float currentTime;

    public GameObject player;
    UnityEngine.Transform playerPos;
    Rigidbody2D rb;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        currentTime = cooldown;
    }

    public void Update()
    {
        currentTime -= Time.deltaTime;
        Vector2 target = new Vector2(playerPos.position.x, playerPos.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {

            Debug.Log("Player Hit");
            collision.TryGetComponent<PlayerStats>(out PlayerStats pStats);
            pStats.PlayerTakeDamage(damage);
        }
    }

    public void EnemyTakeDamage(float damage)
    {
        hp -= damage;
        StartCoroutine(Invulnerability());
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Temporary invulnerability after taking damage
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            sRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            sRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
