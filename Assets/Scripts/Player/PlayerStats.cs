using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 10f;
    public float currentHealth;

    public float swordDamage = 1f;

    // Animation and Sword Reference
    [Header("Sword Reference")]
    private Animator anim;
    public GameObject pSprite;
    private SpriteRenderer sRenderer;
    private Rigidbody2D rb;
    [SerializeField] GameObject swordObj;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;

    public Healthbar healthbar;

    public void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        if (healthbar != null)
        {
            healthbar.SetMaxHealth(maxHealth);
        }

        swordObj.SetActive(false);
    }


    public void Update()
    {
        // Attack Input
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }


    }



    // Activate Sword Attack
    void Attack()
    {
        anim.SetTrigger("Attack");
        rb.linearVelocity = Vector2.zero;
        swordObj.SetActive(true);
    }



    public void HealPlayer(float healAmount)
    {
        currentHealth += healAmount;
        healthbar.SetHealth(currentHealth);
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void PlayerTakeDamage(float damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);
        StartCoroutine(Invulnerability());



        //PUT GAME OVER SCREEN HERE
        if (currentHealth <= 0f)
        {
            Debug.Log("You died");
            EndGame();
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





    public void EndGame()
    {
        SceneManager.LoadScene("GameOverDied");

        // For standalone built versions of the game
        //Application.Quit();

        // For testing inside the Unity Editor
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#endif
    }
}
