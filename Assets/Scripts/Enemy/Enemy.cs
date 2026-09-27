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
    private int facingDirection = 1; // 1 for right, -1 for left

    float currentTime;

    public GameObject player;
    UnityEngine.Transform playerPos;
    Rigidbody2D rb;

    //States
    private EnemyState currentState;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool isPatroling, isChasing, isAttacking;
    public bool isDead;

    //Patrolling
    public float walkPointRange;
    private Vector3 walkPoint;
    private bool walkPointSet;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        sRenderer = GetComponent<SpriteRenderer>();
        ChangeState(EnemyState.Patrol);
        currentTime = cooldown;

        isDead = false;
    }

    public void Update()
    {
        playerInSightRange = Physics2D.OverlapCircle(transform.position, sightRange, LayerMask.GetMask("Player"));
        playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player"));

        if (!playerInSightRange && !playerInAttackRange)
        {
            // Patrol
            isChasing = false;
            isAttacking = false;
            if (!walkPointSet)
            {
                SearchWalkPoint();
            }
            Patrolling();

        }
        if (playerInSightRange && !playerInAttackRange)
        {
            // Chase
            isChasing = true;
            isAttacking = false;
        }
        if (playerInSightRange && playerInAttackRange)
        {
            // Attack
            isChasing = false;
            isAttacking = true;
        }


        if (isChasing == true)
        {
            ChasePlayer();
            if(playerPos.position.x > transform.position.x && facingDirection == -1)
            {
                Flip();
            }
            else if (playerPos.position.x < transform.position.x && facingDirection == 1)
            {
                Flip();
            }
        }


        

}
    public void ChangeState(EnemyState newState)
    {
        currentState = newState;
        //Code for changing animation states here

    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomY = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY, 0);
        Vector3 walkDistance = transform.position - walkPoint;
        if (walkDistance.magnitude >= 0)
        {
            walkPointSet = true;
        }

    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            Vector2 target = new Vector2(walkPoint.x, walkPoint.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        float walkDistance = Vector2.Distance(transform.position, walkPoint);

        if (walkDistance < 1f)
        {
            walkPointSet = false;
        }
    }





    private void ChasePlayer()
    {
        Vector2 target = new Vector2(playerPos.position.x, playerPos.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    private void Flip()
    {
        facingDirection *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}

public enum EnemyState
{
    Patrol,
    Chase,
    Attack
}