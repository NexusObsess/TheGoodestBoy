using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 5f;
    public float speed = 2.5f;
    public float cooldown;
    public float damage = 1f;



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
        }
    }

    public void EnemyTakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

}
