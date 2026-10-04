using System.Collections;
using System.IO;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public string enemyName = "Enemy";

    public float hp = 5f;
    public float speed = 2.5f;
    public float cooldown;
    public float damage = 1f;
    float timer;

    private SpriteRenderer sRenderer;
    private Animator anim;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private int facingDirection = 1; // 1 for right, -1 for left

    float currentTime;

    public GameObject player;
    UnityEngine.Transform playerPos;
    Rigidbody2D rb;

    public GameObject eCollide;
    BoxCollider2D enemyCollider;
    BoxCollider2D attackCollider;

    [SerializeField] float waitTimeOnWayPoint = 1.0f;
    float waitTime;

    [SerializeField] private EnemyShooting shootScript;

    public GameObject healthDrop;

    //States
    public EnemyType attackType;
    private EnemyState currentState;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool isPatroling, isChasing, isAttacking;
    public bool isDead;


    //Patrolling
    public float walkPointRange;
    private Vector3 walkPoint;
    private bool walkPointSet;


    [SerializeField][Range(0f, 1f)] private float dropChance= 0.3f;

    QuestManager questManager;
    GameManager gameManager;


    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform;
        rb = GetComponent<Rigidbody2D>();
        attackCollider = GetComponent<BoxCollider2D>();
        questManager = FindFirstObjectByType<QuestManager>();
        gameManager = FindFirstObjectByType<GameManager>();

        shootScript = GetComponent<EnemyShooting>();

        sRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        attackCollider.enabled = false;

        anim.SetBool("isWalking", false);
        anim.SetBool("isIdle", true);
        ChangeState(EnemyState.Patrol);

        isDead = false;
    }

    public void Update()
    {
        if (gameManager.GameIsPaused) return;
        playerInSightRange = Physics2D.OverlapCircle(transform.position, sightRange, LayerMask.GetMask("Player"));
        playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player"));

        timer += Time.deltaTime;

    

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
            AttackPlayer();
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
        if (gameManager.GameIsPaused) return;
        currentState = newState;
        //Code for changing animation states here

    }
    private void SearchWalkPoint()
    {
        if (gameManager.GameIsPaused) return;
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
        if (gameManager.GameIsPaused) return;
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            Vector2 target = new Vector2(walkPoint.x, walkPoint.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            anim.SetBool("isWalking", true);
            anim.SetBool("isIdle", false);
        }
        float walkDistance = Vector2.Distance(transform.position, walkPoint);

        if (walkDistance < 1f)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", true);
            waitTime += Time.deltaTime;
            if (waitTime >= waitTimeOnWayPoint)
            {
                waitTime = 0.0f;
                walkPointSet = false;
            }
            
        }
    }


    private void ChasePlayer()
    {
        if (gameManager.GameIsPaused) return;
        Vector2 target = new Vector2(playerPos.position.x, playerPos.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }


    private void AttackPlayer()
    {
        if (gameManager.GameIsPaused) return;
        if (timer > cooldown)
        {
            if (attackType == EnemyType.Melee)
            {
                Debug.Log(enemyName + "Attacking");
                anim.SetTrigger("Attack");

                rb.linearVelocity = Vector2.zero;

                timer = 0;
            }
            else if (attackType == EnemyType.Ranged)
            {
                Debug.Log(enemyName + "Shooting");
                anim.SetTrigger("Attack");
                shootScript.Shoot();


                rb.linearVelocity = Vector2.zero;

                timer = 0;
            }

        }
        else
        {

            return;
        }
    }

    public void AttackActive()
    {
        attackCollider.enabled = true;
    }

    public void AttackEnded()
    {
        attackCollider.enabled = false;
    }

    public void HealthDrop()
    {
        if (gameManager.GameIsPaused) return;
        if (Random.value <= dropChance)
        {
            Instantiate(healthDrop, gameObject.transform.position, Quaternion.identity);
        }
    }

    private void Flip()
    {
        if (gameManager.GameIsPaused) return;
        facingDirection *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.GameIsPaused) return;

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
            HealthDrop();

            questManager.HuntCheck(this);
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


    private void ChangeState()
    {
        if(currentState == EnemyState.Patrol)
        {
            // Patrol logic
        }
        else if (currentState == EnemyState.Chase)
        {
            // Chase logic
        }
        else if (currentState == EnemyState.Attack)
        {
            // Attack logic
        }
    }

}
public enum EnemyType
{
    Ranged,
    Melee
}

public enum EnemyState
{
    Patrol,
    Chase,
    Attack
}